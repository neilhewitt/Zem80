using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;
using Zem80.Core;

namespace ZXSpectrum.VM
{
    public class ScreenMap
    {
        private ColourValue _border = DisplayColour.White;
        private PixelMap _pixels;
        private AttributeMap _attributes;
        private byte[] _rgba;
        private Random _random = new Random();
        private IDictionary<int, ushort> _screenLineAddresses;


        public ColourValue BorderColour => _border;
        public PixelMap PixelMap => _pixels;
        public AttributeMap AttributeMap => _attributes;

        public void Fill(byte[] pixels, byte[] attributes)
        {
            for (byte y = 0; y < 192; y++)
            {
                ushort address = _screenLineAddresses[y]; // base address for this screen line
                for (byte x = 0; x < 32; x++)
                {
                    byte pixelData = pixels[address + x];
                    for (int i = 0; i < 8; i++)
                    {
                        _pixels[y, (x * 8) + i] = pixelData.GetBit(7 - i);
                    }
                }
            }

            int columnCounter = 0;
            int rowCounter = 0;
            foreach (byte attribute in attributes)
            {
                // 32 x 24
                AttributeMap[rowCounter, columnCounter] = new DisplayAttribute()
                {
                    Ink = DisplayColour.FromThreeBit(attribute.GetByteFromBits(0, 3)),
                    Paper = DisplayColour.FromThreeBit(attribute.GetByteFromBits(3, 3)),
                    Bright = attribute.GetBit(6),
                    Flash = attribute.GetBit(7)
                };

                columnCounter++;
                if (columnCounter == 32)
                {
                    columnCounter = 0;
                    rowCounter++;
                }
            }
        }

        public byte[] ToRGBA(bool flashOn)
        {
            byte[] pixels = _rgba;
            int pixelIndex = 0;
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 320; x++)
                {
                    for (int rgbIndex = 0; rgbIndex < 4; rgbIndex++)
                    {
                        pixels[pixelIndex] = getPixel(y, x, rgbIndex);
                        pixelIndex += 1;
                    }
                }
            }
            return pixels;

            byte getPixel(int y, int x, int rgbIndex)
            {
                Color pixelColour;
                if (rgbIndex == 3) return 0xFF; // alpha channel is always max

                if (y < 32 || y > 223 || x < 32 || x > 287)
                {
                    // this is the border
                    pixelColour = _border.Normal;
                }
                else
                {
                    x = x - 32; // offset for border
                    y = y - 32;
                    int x8 = (int)(x / 8);
                    int y8 = (int)(y / 8);
                    ScreenPixel pixel = new ScreenPixel(_pixels[y, x], _attributes[y8, x8]);
                    Color ink = pixel.Attribute.Bright ? pixel.Attribute.Ink.Bright : pixel.Attribute.Ink.Normal;
                    Color paper = pixel.Attribute.Bright ? pixel.Attribute.Paper.Bright : pixel.Attribute.Paper.Normal;
                    if (flashOn && pixel.Attribute.Flash)
                    {
                        pixelColour = ink;
                        ink = paper;
                        paper = pixelColour;
                    }
                    pixelColour = pixel.Set ? ink : paper;
                }

                return rgbIndex switch { 0 => pixelColour.B, 1 => pixelColour.G, 2 => pixelColour.R, _ => 0xFF };
            }
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            int pixelIndex = 0;
            for (int y = 0; y < 192; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                        output.Append(getPixel(y, x));
                        pixelIndex += 1;
                }
                output.Append("\n");
            }
            return output.ToString();

            char getPixel(int y, int x)
            {
                ScreenPixel pixel = new ScreenPixel(_pixels[y, x], _attributes[(y / 8), (x / 8)]);
                char output = pixel switch
                {
                    var p when (!p.Set) => ' ',
                    var p when (p.Set) => '.',
                    _ => 'x'
                };

                return output;
            }
        }

        public void SetBorderColour(ColourValue colour)
        {
            _border = colour;
        }

        public ScreenMap()
        {
            _pixels = new PixelMap(192, 256);
            _attributes = new AttributeMap((192 / 8), (256 / 8));
            _rgba = new byte[(192 + 32 + 32) * (256 + 32 + 32) * 4];

            // screen pixel layout is not linear in memory - it's done in 'stripes' across each third of the screen
            // so the memory at 0x4000 contains the 256 bytes for screen line 0, but 0x4100 contains the bytes for 
            // screen line 65 (the first line of the second third of the screen), then 0x4200 contains bytes for screen line 129
            // and finally 0x4300 contains the bytes for screen line 2. This pattern repeats for the whole screen buffer.

            // We pre-calculate the memory address index of each screen line here:
            _screenLineAddresses = new Dictionary<int, ushort>();
            for (byte y = 0; y < 192; y++)
            {
                ushort address = 0x0000;
                address = address.SetBit(8, y.GetBit(0));
                address = address.SetBit(9, y.GetBit(1));
                address = address.SetBit(10, y.GetBit(2));
                address = address.SetBit(5, y.GetBit(3));
                address = address.SetBit(6, y.GetBit(4));
                address = address.SetBit(7, y.GetBit(5));
                address = address.SetBit(11, y.GetBit(6));
                address = address.SetBit(12, y.GetBit(7));
                _screenLineAddresses.Add(y, address);
            }
        }
    }
}

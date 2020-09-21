using System;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;
using Z80.Core;

namespace Z80.ZXSpectrumVM
{
    public class ScreenPixel
    {
        public bool Set { get; private set; }
        public DisplayAttribute Attribute { get; private set; }

        public ScreenPixel(bool set, DisplayAttribute attribute)
        {
            Set = set;
            Attribute = attribute;
        }
    }

    public class ScreenMap
    {
        private ColourValue _border = DisplayColour.White;
        private PixelMap _pixels;
        private AttributeMap _attributes;
        private byte[] _rgba;
        private Random _random = new Random();

        public ColourValue BorderColour => _border;
        public PixelMap PixelMap => _pixels;
        public AttributeMap AttributeMap => _attributes;

        public void SetPixels(int y, int x, byte pixels)
        {
            for (int i = 0; i < 8; i++)
            {
                _pixels[y, x + i] = pixels.GetBit(8 - i);
            }
        }


        public byte[] AsRGBA(bool flash)
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
                    ScreenPixel pixel = new ScreenPixel(_pixels[y, x], _attributes[(y / 8), (x / 8)]);
                    Color ink = pixel.Attribute.Bright ? pixel.Attribute.Ink.Bright : pixel.Attribute.Ink.Normal;
                    Color paper = pixel.Attribute.Bright ? pixel.Attribute.Paper.Bright : pixel.Attribute.Paper.Normal;
                    if (flash && pixel.Attribute.Flash)
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

        public ScreenMap(int height, int width, int border, int attributeHorizontalPixels, int attributeVerticalPixels)
        {
            _pixels = new PixelMap(height, width);
            _attributes = new AttributeMap((height / attributeVerticalPixels), (width / attributeHorizontalPixels));
            _rgba = new byte[(height + border + border) * (width + border + border) * 4];
        }
    }
}

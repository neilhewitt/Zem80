using System;
using System.Linq.Expressions;

namespace Z80.ZXSpectrumVM
{
    public class ScreenMap
    {
        private ColourValue _border;
        private PixelMap _pixels;
        private AttributeMap _attributes;
        private byte[] _rgba;
        private Random _random = new Random();

        public ColourValue BorderColour => _border;
        public PixelMap PixelMap => _pixels;
        public AttributeMap AttributeMap => _attributes;

        public byte[] AsRGBA(bool flash)
        {
            byte[] pixels = _rgba;
            int pixelIndex = 0;
            for (int row = 0; row < 192; row++)
            {
                for (int column = 0; column < 256; column++)
                {
                    for (int rgbIndex = 0; rgbIndex < 4; rgbIndex++)
                    {
                        pixels[pixelIndex++] = getPixel(row, column, rgbIndex);
                    }
                }
            }
            return pixels;

            byte getPixel(int row, int column, int rgbIndex)
            {
                if (rgbIndex == 3) return 0xFF; // alpha channel is always max
                //return (byte)(_random.Next(0, 255)); // for occasional testing :-)

                (bool Set, DisplayAttribute Attribute) pixel = (_pixels[row, column], _attributes[(row / 8), (column / 8)]);
                (byte, byte, byte) ink = pixel.Attribute.Bright ? pixel.Attribute.Ink.Bright : pixel.Attribute.Ink.Normal;
                (byte, byte, byte) paper = pixel.Attribute.Bright ? pixel.Attribute.Paper.Bright : pixel.Attribute.Paper.Normal;
                (byte R, byte G, byte B) pixelColour;
                if (flash)
                {
                    pixelColour = ink;
                    ink = paper;
                    paper = pixelColour;
                }
                pixelColour = pixel.Set ? ink : paper;

                return rgbIndex switch { 0 => pixelColour.B, 1 => pixelColour.G, 2 => pixelColour.R, _ => 0x00 };
            }
        }

        public void SetBorderColour(ColourValue colour)
        {
            _border = colour;
        }

        public ScreenMap(int rows, int columns, int attributeHorizontalPixels, int attributeVerticalPixels)
        {
            _pixels = new PixelMap(rows, columns);
            _attributes = new AttributeMap((rows / attributeVerticalPixels), (columns / attributeHorizontalPixels));
            _rgba = new byte[rows * columns * 4];
        }
    }
}

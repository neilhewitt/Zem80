using System.Drawing;
using Zem80.Core;

namespace ZXSpectrum.VM
{
    public static class DisplayColour
    {
        public static ColourValue FromThreeBit(byte threeBit, int bitIndex)
        {
            threeBit = (byte)((threeBit >> bitIndex) & 0b111);
            foreach (ColourValue colour in new ColourValue[] { Black, Blue, Red, Magenta, Green, Cyan, Yellow, White })
            {
                if (threeBit == colour.Bits) return colour;
            }

            return null;
        }

        public static ColourValue Black { get; } = new ColourValue("Black", 0b000, new ColourParts(0, 0 ,0), new ColourParts(0 ,0 ,0));
        public static ColourValue Blue { get; } = new ColourValue("Blue", 0b001, new ColourParts(0, 0xD7, 0), new ColourParts(0, 0xFF, 0));
        public static ColourValue Red { get; } = new ColourValue("Red", 0b010, new ColourParts(0xD7, 0, 0), new ColourParts(0xFF, 0, 0));
        public static ColourValue Magenta { get; } = new ColourValue("Magenta", 0b011, new ColourParts(0xD7, 0, 0xD7), new ColourParts(0xFF, 0, 0xFF));
        public static ColourValue Green { get; } = new ColourValue("Green", 0b100, new ColourParts(0, 0xD7, 0), new ColourParts(0, 0xFF, 0));
        public static ColourValue Cyan { get; } = new ColourValue("Cyan", 0b101, new ColourParts(0, 0xD7, 0xD7), new ColourParts(0, 0xFF, 0xFF));
        public static ColourValue Yellow { get; } = new ColourValue("Yellow", 0b110, new ColourParts(0xD7, 0xD7, 0), new ColourParts(0xFF, 0xFF, 0));
        public static ColourValue White { get; } = new ColourValue("White", 0b111, new ColourParts(0xD7, 0xD7, 0xD7), new ColourParts(0xFF, 0xFF, 0xFF));

    }
}

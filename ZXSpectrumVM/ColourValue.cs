using System.Drawing;

namespace Z80.ZXSpectrumVM
{
    public class ColourValue
    {
        public string Name { get; private set; }
        public byte Bits { get; private set; }
        public (byte red, byte green, byte blue) Normal { get; private set; }
        public (byte red, byte green, byte blue) Bright { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public ColourValue(string name, byte bits, Color normal, Color bright)
        {
            Name = name;
            Bits = bits;
            Normal = (normal.R, normal.G, normal.B);
            Bright = (bright.R, bright.G, bright.B);
        }
    }
}

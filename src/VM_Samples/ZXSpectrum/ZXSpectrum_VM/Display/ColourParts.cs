namespace ZXSpectrum.VM
{
    public class ColourParts
    {
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }

        public ColourParts(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}

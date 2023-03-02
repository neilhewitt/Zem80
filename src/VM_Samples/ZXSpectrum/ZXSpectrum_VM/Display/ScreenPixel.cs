using System.Drawing;

namespace ZXSpectrum.VM
{
    public class ScreenPixel
    {
        public bool Set { get; private set; }
        public DisplayAttribute Attribute { get; private set; }
        
        public Color GetColor(bool flashOn)
        {
            Color ink = Attribute.Bright ? Attribute.Ink.Bright : Attribute.Ink.Normal;
            Color paper = Attribute.Bright ? Attribute.Paper.Bright : Attribute.Paper.Normal;

            if (Attribute.Flash && flashOn)
            {
                Color third = ink;
                ink = paper;
                paper = third;
            }

            return Set ? ink : paper;
        }

        public ScreenPixel(bool set, DisplayAttribute attribute)
        {
            Set = set;
            Attribute = attribute;
        }
    }
}

namespace ZXSpectrum.VM
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
}

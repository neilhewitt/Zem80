namespace Z80.ZXSpectrumVM
{
    public class AttributeMap
    {
        private DisplayAttribute[,] _attributes;

        public DisplayAttribute this[int row, int column] { get { return _attributes[row, column]; } set { _attributes[row, column] = value; } }

        public AttributeMap(int height, int width)
        {
            _attributes = new DisplayAttribute[height, width];
        }
    }
}

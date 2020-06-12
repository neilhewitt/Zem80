namespace Z80.ZXSpectrumVM
{
    public class PixelMap
    {
        private bool[,] _pixels;

        public bool this[int row, int column] { get { return _pixels[row, column]; } set { _pixels[row, column] = value; } }
        
        public PixelMap(int height, int width)
        {
            _pixels = new bool[height, width];
        }
    }
}

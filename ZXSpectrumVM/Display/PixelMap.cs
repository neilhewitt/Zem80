namespace Z80.ZXSpectrumVM
{
    public class PixelMap
    {
        private bool[,] _pixels;

        public bool this[int y, int x] { get { return _pixels[y, x]; } set { _pixels[y, x] = value; } }
        
        public PixelMap(int height, int width)
        {
            _pixels = new bool[height, width];
        }
    }
}

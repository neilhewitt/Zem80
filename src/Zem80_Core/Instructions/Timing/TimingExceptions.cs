namespace Zem80.Core.Instructions
{
    public class TimingExceptions
    {
        public bool HasConditionalOperandDataReadHigh4 { get; private set; }
        public bool HasMemoryRead4 { get; private set; }
        public bool HasMemoryWrite5 { get; private set; }

        internal TimingExceptions(bool odh4, bool mr4, bool mw5)
        {
            HasConditionalOperandDataReadHigh4 = odh4;
            HasMemoryRead4 = mr4;
            HasMemoryWrite5 = mw5;
        }
    }
}

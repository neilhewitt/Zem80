namespace Z80.Core
{
    public class Port : IPort
    {
        public byte Number { get; private set; }

        public byte ReadByte()
        {
            return 0;
        }

        public void WriteByte(byte output)
        {
            // TODO: make it work :-)
        }

        public void SignalRead()
        {
            // tells port that data is about to be read to data bus
        }

        public void SignalWrite()
        {
            // tells port to read the data bus
        }

        public Port(byte number)
        {
            Number = number;
        }
    }
}

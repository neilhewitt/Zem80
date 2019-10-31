using System;

namespace Z80.Core
{
    public class Port : IPort
    {
        private Func<byte> _read;
        private Action<byte> _write;
        private Action<PortSignal> _signal;

        public byte Number { get; private set; }

        public byte ReadByte()
        {
            return (byte)((_read != null) ? _read() : 0);
        }

        public void WriteByte(byte output)
        {
            if (_write != null) _write(output);
        }

        public void SignalRead()
        {
            // tells port that data is about to be read to data bus
            if (_signal != null) _signal(PortSignal.Read);
        }

        public void SignalWrite()
        {
            // tells port to read the data bus
            if (_signal != null) _signal(PortSignal.Write);
        }

        public void Connect(Func<byte> reader, Action<byte> writer, Action<PortSignal> signaller)
        {
            _read = reader;
            _write = writer;
            _signal = signaller;
        }

        public Port(byte number)
        {
            Number = number;
        }
    }
}

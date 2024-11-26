using System;
using Zem80.Core.CPU;

namespace Zem80.Core.InputOutput
{
    public class Port : IPort
    {
        private Func<byte> _read;
        private Action<byte> _write;
        private Action _signalRead;
        private Action _signalWrite;
        private IProcessorTiming _timing;

        public byte Number { get; private set; }

        public byte ReadByte(bool bc)
        {
            _timing.BeginPortReadCycle(Number, bc);
            byte input = (byte)(_read?.Invoke() ?? 0);
            _timing.EndPortReadCycle(input);
            return input;
        }

        public void WriteByte(byte output, bool bc)
        {
            _timing.BeginPortWriteCycle(output, Number, bc);
            _write?.Invoke(output);
            _timing.EndPortWriteCycle();
        }

        public void SignalRead()
        {
            // tells port that data is about to be read to data bus
            _signalRead?.Invoke();
        }

        public void SignalWrite()
        {
            // tells port to write to the data bus
            _signalWrite?.Invoke();
        }

        public void Connect(Func<byte> reader, Action<byte> writer, Action signalRead, Action signalWrite)
        {
            _read = reader;
            _write = writer;
            _signalRead = signalRead;
            _signalWrite = signalWrite;
        }

        public void Disconnect()
        {
            _read = null;
            _write = null;
            _signalRead = null;
            _signalWrite = null;
        }

        public Port(byte number, IProcessorTiming timing)
        {
            Number = number;
            _timing = timing;
        }
    }
}

using System;

namespace Zem80.Core.IO
{
    public class Port
    {
        private Func<byte> _read;
        private Action<byte> _write;
        private Action _signalRead;
        private Action _signalWrite;
        private Processor _cpu;

        public byte Number { get; private set; }

        public byte ReadByte(bool bc)
        {
            _cpu.InstructionTiming.BeginPortReadCycle(Number, bc);
            byte input = (byte)((_read != null) ? _read() : 0);
            _cpu.InstructionTiming.EndPortReadCycle(input);
            return input;
        }

        public void WriteByte(byte output, bool bc)
        {
            _cpu.InstructionTiming.BeginPortWriteCycle(output, Number, bc);
            if (_write != null) _write(output);
            _cpu.InstructionTiming.EndPortWriteCycle();
        }

        public void SignalRead()
        {
            // tells port that data is about to be read to data bus
            if (_signalRead != null)
            {
                _signalRead();
            }
        }

        public void SignalWrite()
        {
            // tells port to read the data bus
            if (_signalWrite != null) 
            {
                _signalWrite(); 
            }
        }

        public void Connect(Func<byte> reader, Action<byte> writer, Action signalRead, Action signalWrite)
        {
            _read = reader;
            _write = writer;
            _signalRead = signalRead;
            _signalWrite = signalWrite;
        }

        public Port(byte number, Processor cpu)
        {
            Number = number;
            _cpu = cpu;
        }
    }
}

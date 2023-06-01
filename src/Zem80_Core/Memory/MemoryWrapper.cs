using System;
using Zem80.Core.CPU;

namespace Zem80.Core.Memory
{
    public class MemoryWrapper : IMemory
    {
        private bool _timed;
        private MemoryBank _memoryBank;
        private Instruction _instruction;

        public byte ReadByteAt(ushort address)
        {
            return _memoryBank.ReadByteAt(address, _timed, ExtraTStates());
        }

        public byte[] ReadBytesAt(ushort address, ushort numberOfBytes)
        {
            return _memoryBank.ReadBytesAt(address, numberOfBytes, _timed, ExtraTStates());
        }

        public ushort ReadWordAt(ushort address)
        {
            return _memoryBank.ReadWordAt(address, _timed, ExtraTStates());
        }

        public void WriteByteAt(ushort address, byte value)
        {
            _memoryBank.WriteByteAt(address, value, _timed, ExtraTStates());
        }

        public void WriteBytesAt(ushort address, byte[] bytes)
        {
            _memoryBank.WriteBytesAt(address, bytes, _timed, ExtraTStates());
        }

        public void WriteWordAt(ushort address, ushort value)
        {
            _memoryBank.WriteWordAt(address, value, _timed, ExtraTStates());
        }

        private byte ExtraTStates()
        {
            return 0;
        }

        public MemoryWrapper(MemoryBank memoryBank, bool timed, Instruction instruction = null)
        {
            _timed = timed;
            _memoryBank = memoryBank;
            _instruction = instruction;
        }
    }
}

using Zem80.Core.Instructions;

namespace Zem80.Core.Memory
{
    public class MemoryWrapper : IMemory
    {
        private bool _timed;
        private MemoryBank _memoryBank;
        private Instruction _instruction;

        public byte ReadByteAt(ushort address)
        {
            return _memoryBank.ReadByteAt(address, _timed);
        }

        public byte[] ReadBytesAt(ushort address, ushort numberOfBytes)
        {
            return _memoryBank.ReadBytesAt(address, numberOfBytes, _timed);
        }

        public ushort ReadWordAt(ushort address)
        {
            return _memoryBank.ReadWordAt(address, _timed);
        }

        public void WriteByteAt(ushort address, byte value)
        {
            _memoryBank.WriteByteAt(address, value, _timed);
        }

        public void WriteBytesAt(ushort address, byte[] bytes)
        {
            _memoryBank.WriteBytesAt(address, bytes, _timed);
        }

        public void WriteWordAt(ushort address, ushort value)
        {
            _memoryBank.WriteWordAt(address, value, _timed);
        }

        public MemoryWrapper(MemoryBank memoryBank, bool timed, Instruction instruction = null)
        {
            _timed = timed;
            _memoryBank = memoryBank;
            _instruction = instruction;
        }
    }
}

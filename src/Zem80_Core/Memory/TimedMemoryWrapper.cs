using Zem80.Core.CPU;

namespace Zem80.Core.Memory
{
    public class TimedMemoryWrapper : IMemory
    {
        private IMemoryBank _wrappedMemory;
        private MachineCycle _cycleForTiming;

        public byte ReadByteAt(ushort address)
        {
            return _wrappedMemory.ReadByteAt(address, TStates());
        }

        public byte[] ReadBytesAt(ushort address, ushort numberOfBytes)
        {
            return _wrappedMemory.ReadBytesAt(address, numberOfBytes, TStates());
        }

        public ushort ReadWordAt(ushort address)
        {
            return _wrappedMemory.ReadWordAt(address, TStates());
        }

        public void WriteByteAt(ushort address, byte value)
        {
            _wrappedMemory.WriteByteAt(address, value, TStates());
        }

        public void WriteBytesAt(ushort address, byte[] bytes)
        {
            _wrappedMemory.WriteBytesAt(address, bytes, TStates());
        }

        public void WriteWordAt(ushort address, ushort value)
        {
            _wrappedMemory.WriteWordAt(address, value, TStates());
        }

        private byte TStates() => _cycleForTiming.TStates;

        public TimedMemoryWrapper(IMemoryBank memoryToWrap, MachineCycle cycleForTiming)
        {
            _wrappedMemory = memoryToWrap;
            _cycleForTiming = cycleForTiming;
        }
    }
}

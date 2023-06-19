using Zem80.Core.CPU;

namespace Zem80.Core.Memory
{
    public interface IMemoryBank
    {
        IMemory Untimed { get; }
        uint SizeInBytes { get; }
        void Clear();
        void Initialise(Processor cpu, IMemoryMap map);
        byte ReadByteAt(ushort address, byte tStates);
        byte[] ReadBytesAt(ushort address, ushort numberOfBytes, byte tStatesPerByte);
        ushort ReadWordAt(ushort address, byte tStatesPerByte);
        void WriteByteAt(ushort address, byte value, byte tStates);
        void WriteBytesAt(ushort address, byte[] bytes, byte tStatesPerByte);
        void WriteWordAt(ushort address, ushort value, byte tStatesPerByte);
    }
}
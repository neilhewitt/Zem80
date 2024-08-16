using Zem80.Core.CPU;

namespace Zem80.Core.Memory
{
    public interface IMemoryBank
    {
        uint SizeInBytes { get; }
        void Clear();
        void Initialise(Processor cpu, IMemoryMap map);
        byte ReadByteAt(ushort address, byte? tStates = null);
        byte[] ReadBytesAt(ushort address, ushort numberOfBytes, byte? tStatesPerByte = null);
        ushort ReadWordAt(ushort address, byte? tStatesPerByte = null);
        void WriteByteAt(ushort address, byte value, byte? tStates = null);
        void WriteBytesAt(ushort address, byte[] bytes, byte? tStatesPerByte = null);
        void WriteWordAt(ushort address, ushort value, byte? tStatesPerByte = null);
    }
}
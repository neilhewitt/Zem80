namespace Zem80.Core.Memory
{
    public interface ITimedMemory
    {
        byte ReadByteAt(ushort address);
        byte[] ReadBytesAt(ushort address, ushort numberOfBytes);
        ushort ReadWordAt(ushort address);
        void WriteByteAt(ushort address, byte value);
        void WriteBytesAt(ushort address, byte[] bytes);
        void WriteWordAt(ushort address, ushort value);
    }
}
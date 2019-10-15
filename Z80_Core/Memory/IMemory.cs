namespace Z80.Core
{
    public interface IMemory
    {
        uint SizeInKilobytes { get; }
        byte ReadByteAt(uint address);
        byte[] ReadBytesAt(uint address, uint numberOfBytes);
        ushort ReadWordAt(uint address);
        void WriteByteAt(uint address, byte value);
        void WriteBytesAt(uint address, params byte[] bytes);
        void WriteWordAt(uint address, ushort value);
    }
}
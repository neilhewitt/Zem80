﻿namespace Z80.Core
{
    public interface IMemory
    {
        int SizeInBytes { get; }
        byte ReadByteAt(ushort address);
        byte[] ReadBytesAt(ushort address, ushort numberOfBytes);
        ushort ReadWordAt(ushort address);
        void WriteByteAt(ushort address, byte value);
        void WriteBytesAt(ushort address, params byte[] bytes);
        void WriteWordAt(ushort address, ushort value);
        void Initialise(Processor cpu);
        void Clear();
    }
}
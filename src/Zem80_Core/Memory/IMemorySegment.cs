using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Memory
{
    public interface IMemorySegment
    {
        ushort StartAddress { get; }
        uint SizeInBytes { get; }
        bool ReadOnly { get; }
        byte ReadByteAt(ushort offset);
        byte[] ReadBytesAt(ushort offset, int bytes);
        void WriteByteAt(ushort offset, byte value);
        void WriteBytesAt(ushort offset, byte[] bytes);
        void MapAt(ushort address);

        void Clear();
    }
}

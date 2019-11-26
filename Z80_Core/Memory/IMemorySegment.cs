using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public interface IMemorySegment
    {
        ushort StartAddress { get; }
        uint SizeInBytes { get; }
        bool ReadOnly { get; }
        byte ReadByteAt(ushort offset);
        void WriteByteAt(ushort offset, byte value);
        void Clear();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public interface IMemorySegment
    {
        ushort StartAddress { get; }
        int SizeInBytes { get; }
        bool ReadOnly { get; }
        byte ReadByteAt(int offset);
        void WriteByteAt(int offset, byte value);
        void Clear();
    }
}

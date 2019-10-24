using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public interface IMemorySegment
    {
        ushort StartAddress { get; }
        ushort SizeInKilobytes { get; }
        bool ReadOnly { get; }
        byte ReadByteAt(ushort address);
        ushort ReadWordAt(ushort address);
        void WriteByteAt(ushort address, byte value);
        void WriteWordAt(ushort address, ushort value);
    }
}

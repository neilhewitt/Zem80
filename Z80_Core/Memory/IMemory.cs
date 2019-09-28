using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public interface IMemory
    {
        uint startAddress { get; }
        uint sizeInKilobytes { get; }
        bool ReadOnly { get; }
        byte ReadByteAt(uint address);
        ushort ReadWordAt(uint address);
        void WriteByteAt(uint address, byte value);
        void WriteWordAt(uint address, ushort value);
    }
}

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
        void WriteByteAt(ushort offset, byte value);
        void MapAt(ushort address);

        void Clear();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.Core
{
    public class ROM : RAM
    {
        new public bool ReadOnly => true;

        public override void WriteByteAt(uint address, byte value)
        {
            throw new MemoryNotWritableException();
        }

        public override void WriteWordAt(uint address, ushort value)
        {
            throw new MemoryNotWritableException();
        }

        public ROM(uint startAddress, uint sizeInKilobytes)
            : base(startAddress, sizeInKilobytes)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.SimpleVM
{
    public class ROM : RAM
    {
        new public bool ReadOnly => true;

        public override void WriteByteAt(uint address, byte value)
        {
            throw new InvalidOperationException();
        }

        public override void WriteWordAt(uint address, ushort value)
        {
            throw new InvalidOperationException();
        }

        public ROM(uint startAddress, uint sizeInKilobytes)
            : base(startAddress, sizeInKilobytes)
        {
        }
    }
}

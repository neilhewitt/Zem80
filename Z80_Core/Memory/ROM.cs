using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.Core
{
    public class ROM : RAM
    {
        new public bool ReadOnly => true;

        public override void WriteByteAt(ushort address, byte value)
        {
            throw new MemoryNotWritableException();
        }

        public override void WriteWordAt(ushort address, ushort value)
        {
            throw new MemoryNotWritableException();
        }

        public ROM(ushort startAddress, ushort sizeInKilobytes)
            : base(startAddress, sizeInKilobytes)
        {
        }
    }
}

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

        new public void Clear()
        {
            // do nothing - we're read-only
        }

        public ROM(ushort address, ushort sizeInBytes)
            : base(address, sizeInBytes)
        {
        }
    }
}

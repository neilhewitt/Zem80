using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core;

namespace Zem80.Core.Memory
{
    public class ReadOnlyMemorySegment : MemorySegment
    {
        new public bool ReadOnly => true;

        public override void WriteByteAt(ushort address, byte value)
        {
            // do nothing
        }

        new public void Clear()
        {
            // do nothing - we're read-only
        }

        public ReadOnlyMemorySegment(byte[] contents)
            : base((uint)contents.Length, contents)
        {
        }
    }
}

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
            throw new MemoryNotWritableException();
        }

        public override void WriteBytesAt(ushort offset, byte[] bytes)
        {
            throw new MemoryNotWritableException();
        }

        new public void Clear()
        {
            // do nothing - we're read-only
        }

        public ReadOnlyMemorySegment(ushort address, byte[] contents)
            : base(address, (uint)contents.Length)
        {
            base.WriteBytesAt(0, contents);
        }
    }
}

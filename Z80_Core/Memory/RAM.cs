using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.Core
{
    public class RAM : IMemorySegment
    {
        private const uint MAX_SEGMENT_SIZE_IN_BYTES = 65536;
        private byte[] _memory;

        public ushort StartAddress { get; private set; }

        public uint SizeInBytes { get; private set; }

        public bool ReadOnly => false;

        public byte ReadByteAt(ushort offset)
        {
            Check(offset);
            return _memory[offset];
        }

        public virtual void WriteByteAt(ushort offset, byte value)
        {
            Check(offset);
            _memory[offset] = value;
        }

        public void Clear()
        {
            _memory = new byte[SizeInBytes];
        }

        private void Check(ushort offset)
        {
            if (offset >= SizeInBytes)
            {
                throw new MemorySegmentException("Specified address is outside the scope of this segment.");
            }
        }

        public RAM(ushort startAddress, uint sizeInBytes)
        {
            // we have to use uint as the size type because the max permissible size is 65536 bytes which cannot be contained in a ushort - but we now need to do a size check
            if (sizeInBytes > MAX_SEGMENT_SIZE_IN_BYTES) throw new MemorySegmentException("Requested segment size exceeded maximum possible size of " + MAX_SEGMENT_SIZE_IN_BYTES + " bytes.");

            _memory = new byte[sizeInBytes];
            StartAddress = startAddress;
            SizeInBytes = sizeInBytes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.Core
{
    public class RAM : IMemorySegment
    {
        private byte[] _memory;

        public ushort StartAddress { get; private set; }

        public int SizeInBytes { get; private set; }

        public bool ReadOnly => false;

        public byte ReadByteAt(int offset)
        {
            Check(offset);
            return _memory[offset];
        }

        public virtual void WriteByteAt(int offset, byte value)
        {
            Check(offset);
            _memory[offset] = value;
        }

        public void Clear()
        {
            _memory = new byte[SizeInBytes];
        }

        private void Check(int offset)
        {
            if (offset < 0 || offset >= SizeInBytes)
            {
                throw new MemorySegmentException("Specified address is outside the scope of this segment.");
            }
        }

        public RAM(ushort startAddress, int sizeInBytes)
        {
            _memory = new byte[sizeInBytes];
            StartAddress = startAddress;
            SizeInBytes = sizeInBytes;

            if (SizeInBytes > 65536)
            {
                throw new MemorySegmentException("Memory segment size cannot exceed 65536 bytes.");
            }
        }
    }
}

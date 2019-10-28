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

        public uint SizeInBytes { get; private set; }

        public bool ReadOnly => false;

        public byte ReadByteAt(ushort address)
        {
            Check(address);
            return _memory[address - StartAddress];
        }

        public virtual void WriteByteAt(ushort address, byte value)
        {
            Check(address);
            _memory[address - StartAddress] = value;
        }

        private void Check(ushort address)
        {
            if (address < StartAddress || address >= (StartAddress + SizeInBytes))
            {
                throw new MemorySegmentException("Specified address is outside the scope of this segment.");
            }
        }

        public RAM(ushort startAddress, uint sizeInBytes)
        {
            _memory = new byte[sizeInBytes];
            SizeInBytes = sizeInBytes;
            StartAddress = startAddress;

            if (SizeInBytes > 65536)
            {
                throw new MemorySegmentException("Memory segment size cannot exceed 65536 bytes.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.Core
{
    public class RAM : IMemorySegment
    {
        private byte[] _memory;

        public uint StartAddress { get; private set; }

        public uint SizeInKilobytes { get; private set; }

        public bool ReadOnly => false;

        public byte ReadByteAt(uint address)
        {
            Check(address);
            return _memory[address - StartAddress];
        }

        public ushort ReadWordAt(uint address)
        {
            Check(address);
            uint relativeAddress = address - StartAddress;
            return (ushort)((_memory[relativeAddress + 1] * 256) + _memory[relativeAddress]); // always little-endian
        }

        public virtual void WriteByteAt(uint address, byte value)
        {
            Check(address);
            _memory[address - StartAddress] = value;
        }

        public virtual void WriteWordAt(uint address, ushort value)
        {
            Check(address);
            uint relativeAddress = address - StartAddress;
            byte msb = (byte)(value / 256); // most significant byte (little-endian regardless of architecture)
            byte lsb = (byte)(value - msb); // least signifcant byte ("")
            _memory[relativeAddress] = lsb;
            _memory[relativeAddress + 1] = msb;
        }

        private void Check(uint address)
        {
            if (address < StartAddress || address >= (StartAddress + (SizeInKilobytes * 1024)))
            {
                throw new MemorySegmentException("Specified address is outside the scope of this segment.");
            }
        }

        public RAM(uint startAddress, uint sizeInKilobytes)
        {
            _memory = new byte[sizeInKilobytes * 1024];
            SizeInKilobytes = sizeInKilobytes;
            StartAddress = startAddress;
        }
    }
}

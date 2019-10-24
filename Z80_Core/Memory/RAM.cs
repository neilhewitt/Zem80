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

        public ushort SizeInKilobytes { get; private set; }

        public bool ReadOnly => false;

        public byte ReadByteAt(ushort address)
        {
            Check(address);
            return _memory[address - StartAddress];
        }

        public ushort ReadWordAt(ushort address)
        {
            Check(address);
            ushort relativeAddress = (ushort)(address - StartAddress);
            return (ushort)((_memory[relativeAddress + 1] * 256) + _memory[relativeAddress]); // always little-endian
        }

        public virtual void WriteByteAt(ushort address, byte value)
        {
            Check(address);
            _memory[address - StartAddress] = value;
        }

        public virtual void WriteWordAt(ushort address, ushort value)
        {
            Check(address);
            ushort relativeAddress = (ushort)(address - StartAddress);
            byte msb = (byte)(value / 256); // most significant byte (little-endian regardless of architecture)
            byte lsb = (byte)(value - msb); // least signifcant byte ("")
            _memory[relativeAddress] = lsb;
            _memory[relativeAddress + 1] = msb;
        }

        private void Check(ushort address)
        {
            if (address < StartAddress || address >= (StartAddress + (SizeInKilobytes * 1024)))
            {
                throw new MemorySegmentException("Specified address is outside the scope of this segment.");
            }
        }

        public RAM(ushort startAddress, ushort sizeInKilobytes)
        {
            _memory = new byte[sizeInKilobytes * 1024];
            SizeInKilobytes = sizeInKilobytes;
            StartAddress = startAddress;
        }
    }
}

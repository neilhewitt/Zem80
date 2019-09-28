using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.SimpleVM
{
    public class RAM : IMemory
    {
        public bool ReadOnly => throw new NotImplementedException();

        public uint startAddress => throw new NotImplementedException();

        public uint sizeInKilobytes => throw new NotImplementedException();

        public byte ReadByteAt(uint address)
        {
            throw new NotImplementedException();
        }

        public ushort ReadWordAt(uint address)
        {
            throw new NotImplementedException();
        }

        public void WriteByteAt(uint address, byte value)
        {
            throw new NotImplementedException();
        }

        public void WriteWordAt(uint address, ushort value)
        {
            throw new NotImplementedException();
        }
    }
}

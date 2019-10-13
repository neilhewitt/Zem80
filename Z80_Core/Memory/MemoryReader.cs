using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class MemoryReader
    {
        private MemoryMap _map;

        public byte ReadByteAt(uint address)
        {
            IMemoryLocation memory = _map.MemoryFor(address);
            return memory?.ReadByteAt(address) ?? 255; // default value if address is unallocated
        }

        public byte[] ReadBytesAt(uint address, uint numberOfBytes)
        {
            IMemoryLocation memory = _map.MemoryFor(address);
            byte[] bytes = new byte[numberOfBytes];
            for (uint i = 0; i < numberOfBytes; i++)
            {
                bytes[i] = memory?.ReadByteAt(address + i) ?? 255;
            }
            return bytes;
        }

        public ushort ReadWordAt(uint address)
        {
            IMemoryLocation memory = _map.MemoryFor(address);
            return memory?.ReadWordAt(address) ?? 65535; // default value if address is unallocated
        }

        public MemoryReader(MemoryMap map)
        {
            _map = map;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class Memory : IMemory
    {
        private const int DEFAULT_MEMORY_SIZE_IN_KILOBYTES = 16;
        private IMemoryMap _map;

        public uint SizeInKilobytes => _map.SizeInKilobytes;

        public byte ReadByteAt(uint address)
        {
            IMemorySegment memory = _map.MemoryFor(address);
            return memory?.ReadByteAt(address) ?? 255; // default value if address is unallocated
        }

        public byte[] ReadBytesAt(uint address, uint numberOfBytes)
        {
            IMemorySegment memory = _map.MemoryFor(address);
            byte[] bytes = new byte[numberOfBytes];
            for (uint i = 0; i < numberOfBytes; i++)
            {
                bytes[i] = memory?.ReadByteAt(address + i) ?? 255;
            }
            return bytes;
        }

        public ushort ReadWordAt(uint address)
        {
            IMemorySegment memory = _map.MemoryFor(address);
            return memory?.ReadWordAt(address) ?? 65535; // default value if address is unallocated
        }

        public void WriteByteAt(uint address, byte value)
        {
            IMemorySegment memory = _map.MemoryFor(address);
            if (memory == null || memory.ReadOnly)
            {
                throw new Exception("Readonly"); // TODO: custom exception type
            }

            memory.WriteByteAt(address, value);
        }

        public void WriteBytesAt(uint address, params byte[] bytes)
        {
            IMemorySegment memory = _map.MemoryFor(address);
            if (memory == null || memory.ReadOnly)
            {
                throw new Exception("Readonly"); // TODO: custom exception type
            }

            for (uint i = 0; i < bytes.Length; i++)
            {
                memory.WriteByteAt(address + i, bytes[i]);
            }
        }

        public void WriteWordAt(uint address, ushort value)
        {
            IMemorySegment memory = _map.MemoryFor(address);
            if (memory == null || memory.ReadOnly)
            {
                throw new Exception("Readonly"); // TODO: custom exception type
            }

            memory.WriteWordAt(address, value);
        }

        public Memory(IMemoryMap map)
        {
            _map = map;
        }
    }
}

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
            return memory?.ReadByteAt(address) ?? 0; // default value if address is unallocated
        }

        public byte[] ReadBytesAt(uint address, uint numberOfBytes)
        {
            byte[] bytes = new byte[numberOfBytes];
            for (uint i = 0; i < numberOfBytes; i++)
            {
                bytes[i] = ReadByteAt(address + i);
            }
            return bytes;
        }

        public ushort ReadWordAt(uint address)
        {
            byte[] bytes = ReadBytesAt(address, 2);
            return (ushort)((bytes[1] * 256) + bytes[0]);
        }

        public void WriteByteAt(uint address, byte value)
        {
            IMemorySegment memory = _map.MemoryFor(address);
            if (memory == null || memory.ReadOnly)
            {
                throw new Exception("Readonly or unmapped"); // TODO: custom exception type
            }

            memory.WriteByteAt(address, value);
        }

        public void WriteBytesAt(uint address, params byte[] bytes)
        {
            for (uint i = 0; i < bytes.Length; i++)
            {
                WriteByteAt(address + i, bytes[i]);
            }
        }

        public void WriteWordAt(uint address, ushort value)
        {
            byte[] bytes = new byte[2];
            bytes[1] = (byte)(value / 256);
            bytes[0] = (byte)(value % 256);

            WriteBytesAt(address, bytes);
        }

        public Memory(IMemoryMap map)
        {
            _map = map;
        }
    }
}

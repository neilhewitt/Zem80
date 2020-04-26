using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class Memory
    {
        private IMemoryMap _map;
        private Processor _cpu;
        private bool _initialised;

        public uint SizeInBytes => _map.SizeInBytes;

        public byte ReadByteAt(ushort address, bool suppressMachineCycle = false)
        {
            if (!_initialised) throw new MemoryException();
            IMemorySegment segment = _map.MemoryFor(address);
            byte output = (segment?.ReadByteAt((ushort)(address - segment.StartAddress)) ?? 0x00); // default value if address is unallocated
            if (!suppressMachineCycle) _cpu.MemoryReadCycle((ushort)(address - segment.StartAddress), output);
            return output;
        }

        public byte[] ReadBytesAt(ushort address, ushort numberOfBytes, bool suppressMachineCycle = false)
        {
            uint availableBytes = numberOfBytes;
            if (address + availableBytes >= SizeInBytes) availableBytes = SizeInBytes - address; // if this read overflows the end of memory, we can read only this many bytes

            byte[] bytes = new byte[numberOfBytes];
            for (int i = 0; i < availableBytes; i++)
            {
                bytes[i] = ReadByteAt((ushort)(address + i), suppressMachineCycle);
            }
            return bytes; // bytes beyond the available byte limit (if any) will be 0x00
        }

        public ushort ReadWordAt(ushort address, bool suppressMachineCycle = false)
        {
            byte[] bytes = ReadBytesAt(address, 2, suppressMachineCycle);
            return (ushort)((bytes[1] * 256) + bytes[0]);
        }

        public void WriteByteAt(ushort address, byte value, bool suppressMachineCycle = false)
        {
            if (!_initialised) throw new MemoryException();
            IMemorySegment segment = _map.MemoryFor(address);
            if (segment == null || segment.ReadOnly)
            {
                throw new MemoryNotPresentException("Readonly or unmapped");
            }
            segment.WriteByteAt((ushort)(address - segment.StartAddress), value);
            if (!suppressMachineCycle) _cpu.MemoryWriteCycle((ushort)(address - segment.StartAddress), value);
        }

        public void WriteBytesAt(ushort address, byte[] bytes, bool suppressMachineCycle = false)
        {
            for (ushort i = 0; i < bytes.Length; i++)
            {
                WriteByteAt((ushort)(address + i), bytes[i], suppressMachineCycle);
            }
        }

        public void WriteWordAt(ushort address, ushort value, bool suppressMachineCycle = false)
        {
            byte[] bytes = new byte[2];
            bytes[1] = (byte)(value / 256);
            bytes[0] = (byte)(value % 256);

            WriteBytesAt(address, bytes, suppressMachineCycle);
        }

        public void Initialise(Processor cpu, IMemoryMap map)
        {
            _cpu = cpu;
            _map = map;
            _initialised = true;
        }

        public void Clear()
        {
            _map.ClearAllWritableMemory();
        }

        public Memory()
        {
        }
    }
}

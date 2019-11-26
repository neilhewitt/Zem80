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

        public int SizeInBytes => _map.SizeInBytes;

        public byte ReadByteAt(ushort address)
        {
            if (!_initialised) throw new MemoryException();
            _cpu.SetAddressBus(address);

            IMemorySegment segment = _map.MemoryFor(address);
            _cpu.SetDataBus(segment?.ReadByteAt((ushort)(address - segment.StartAddress)) ?? 0x00); // default value if address is unallocated
            return _cpu.DataBus;
        }

        public byte[] ReadBytesAt(ushort address, ushort numberOfBytes)
        {
            int availableBytes = numberOfBytes;
            if (address + availableBytes >= SizeInBytes) availableBytes = SizeInBytes - address; // if this read overflows the end of memory, we can read only this many bytes

            byte[] bytes = new byte[numberOfBytes];
            for (int i = 0; i < availableBytes; i++)
            {
                bytes[i] = ReadByteAt((ushort)(address + i));
            }
            return bytes; // bytes beyond the available byte limit (if any) will be 0x00
        }

        public ushort ReadWordAt(ushort address)
        {
            byte[] bytes = ReadBytesAt(address, 2);
            return (ushort)((bytes[1] * 256) + bytes[0]);
        }

        public void WriteByteAt(ushort address, byte value)
        {
            if (!_initialised) throw new MemoryException();
            _cpu.SetAddressBus(address);

            IMemorySegment segment = _map.MemoryFor(address);
            if (segment == null || segment.ReadOnly)
            {
                throw new MemoryNotPresentException("Readonly or unmapped");
            }

            _cpu.SetDataBus(value);
            segment.WriteByteAt((ushort)(address - segment.StartAddress), value);
        }

        public void WriteBytesAt(ushort address, params byte[] bytes)
        {
            for (ushort i = 0; i < bytes.Length; i++)
            {
                WriteByteAt((ushort)(address + i), bytes[i]);
            }
        }

        public void WriteWordAt(ushort address, ushort value)
        {
            byte[] bytes = new byte[2];
            bytes[1] = (byte)(value / 256);
            bytes[0] = (byte)(value % 256);

            WriteBytesAt(address, bytes);
        }

        public void Initialise(Processor cpu)
        {
            _cpu = cpu;
            _initialised = true;
        }

        public void Clear()
        {
            _map.ClearAllWritableMemory();
        }

        public Memory(IMemoryMap map)
        {
            _map = map;
        }
    }
}

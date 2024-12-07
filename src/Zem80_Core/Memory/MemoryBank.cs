using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using Zem80.Core.CPU;

namespace Zem80.Core.Memory
{
    public class MemoryBank : IMemoryBank
    {
        internal IMemoryMap _map;
        internal Processor _cpu;
        internal bool _initialised;

        public uint SizeInBytes => _map.SizeInBytes;

        public void Clear()
        {
            _map.ClearAllWritableMemory();
        }

        public void Initialise(Processor cpu, IMemoryMap map)
        {
            _cpu = cpu;
            _map = map;
            _initialised = true;
        }

        public byte ReadByteAt(ushort address, byte? tStates)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            IMemorySegment segment = _map.SegmentFor(address);
            byte output = segment?.ReadByteAt(AddressOffset(address, segment)) ?? 0x00; // 0x00 if address is unallocated
            if (tStates > 0) _cpu.Timing.MemoryReadCycle(address, output, tStates.Value);
            return output;
        }

        public void WriteByteAt(ushort address, byte value, byte? tStates)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            IMemorySegment segment = _map.SegmentFor(address);
            if (segment != null && !segment.ReadOnly)
            {
                segment.WriteByteAt(AddressOffset(address, segment), value);
            }

            if (tStates > 0) _cpu.Timing.MemoryWriteCycle(address, value, tStates.Value);
        }

        public ushort ReadWordAt(ushort address, byte? tStatesPerByte)
        {
            byte low = ReadByteAt(address, tStatesPerByte);
            byte high = ReadByteAt(++address, tStatesPerByte);
            return (ushort)((high * 256) + low);
        }

        public void WriteWordAt(ushort address, ushort value, byte? tStatesPerByte)
        {
            WriteByteAt(address, (byte)(value % 256), tStatesPerByte);
            WriteByteAt(++address, (byte)(value / 256), tStatesPerByte);
        }

        public byte[] ReadBytesAt(ushort address, ushort numberOfBytes, byte? tStatesPerByte)
        {
            uint availableBytes = numberOfBytes;
            if (address + availableBytes >= SizeInBytes) availableBytes = SizeInBytes - address; // if this read overflows the end of memory, we can read only this many bytes

            byte[] bytes = new byte[numberOfBytes];
            for (int i = 0; i < availableBytes; i++)
            {
                bytes[i] = ReadByteAt((ushort)(address + i), tStatesPerByte);
            }
            return bytes; // bytes beyond the available byte limit (if any) will be 0x00
        }

        public void WriteBytesAt(ushort address, byte[] bytes, byte? tStatesPerByte)
        {
            for (ushort i = 0; i < bytes.Length; i++)
            {
                WriteByteAt((ushort)(address + i), bytes[i], tStatesPerByte);
            }
        }

        private ushort AddressOffset(ushort address, IMemorySegment segment)
        {
            return (ushort)(address - segment.StartAddress);
        }

        public MemoryBank()
        {
        }
    }
}

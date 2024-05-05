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

        private IMemoryBank Me => this as IMemoryBank;

        byte IMemoryBank.ReadByteAt(ushort address, byte? tStates)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            IMemorySegment segment = _map.SegmentFor(address);
            byte output = segment?.ReadByteAt(AddressOffset(address, segment)) ?? 0x00; // 0x00 if address is unallocated
            if (tStates > 0) _cpu.Timing.MemoryReadCycle(address, output, tStates.Value);
            return output;
        }

        byte[] IMemoryBank.ReadBytesAt(ushort address, ushort numberOfBytes, byte? tStatesPerByte)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            uint availableBytes = numberOfBytes;
            if (address + availableBytes >= SizeInBytes) availableBytes = SizeInBytes - address; // if this read overflows the end of memory, we can read only this many bytes

            IMemorySegment segment = _map.SegmentFor(address);
            if (segment == null) return new byte[numberOfBytes]; // if memory is not allocated return all 0x00s

            byte[] bytes = new byte[numberOfBytes];
            for (int i = 0; i < availableBytes; i++)
            {
                bytes[i] = Me.ReadByteAt((ushort)(address + i), tStatesPerByte);
            }
            return bytes; // bytes beyond the available byte limit (if any) will be 0x00
        }

        ushort IMemoryBank.ReadWordAt(ushort address, byte? tStatesPerByte)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            byte low = Me.ReadByteAt(address, tStatesPerByte);
            byte high = Me.ReadByteAt(++address, tStatesPerByte);
            return (ushort)((high * 256) + low);
        }

        void IMemoryBank.WriteByteAt(ushort address, byte value, byte? tStates)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            IMemorySegment segment = _map.SegmentFor(address);
            if (segment != null && !segment.ReadOnly)
            {
                segment.WriteByteAt(AddressOffset(address, segment), value);
            }

            if (tStates > 0) _cpu.Timing.MemoryWriteCycle(address, value, tStates.Value);
        }

        void IMemoryBank.WriteBytesAt(ushort address, byte[] bytes, byte? tStatesPerByte)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            IMemorySegment segment = _map.SegmentFor(address);
            if (segment != null && !segment.ReadOnly)
            {
                for (ushort i = 0; i < bytes.Length; i++)
                {
                    Me.WriteByteAt((ushort)(address + i), bytes[i], tStatesPerByte);
                }
            }
        }

        void IMemoryBank.WriteWordAt(ushort address, ushort value, byte? tStatesPerByte)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            Me.WriteByteAt(address, (byte)(value % 256), tStatesPerByte);
            Me.WriteByteAt(++address, (byte)(value / 256), tStatesPerByte);
        }


        public ushort AddressOffset(ushort address, IMemorySegment segment)
        {
            return (ushort)(address - segment.StartAddress);
        }

        public MemoryBank()
        {
        }
    }
}

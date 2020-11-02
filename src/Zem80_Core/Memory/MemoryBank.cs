using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace Zem80.Core.Memory
{
    public class MemoryBank
    {
        private IMemoryMap _map;
        private Processor _cpu;
        private bool _initialised;

        public uint SizeInBytes => _map.SizeInBytes;

        public byte ReadByteAt(ushort address, bool noTiming)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            IMemorySegment segment = _map.SegmentFor(address);
            byte output = (segment?.ReadByteAt(AddressOffset(address, segment)) ?? 0x00); // 0x00 if address is unallocated
            if (!noTiming) _cpu.Cycle.MemoryReadCycle(AddressOffset(address, segment), output);
            return output;
        }

        public byte[] ReadBytesAt(ushort address, ushort numberOfBytes, bool noTiming)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            uint availableBytes = numberOfBytes;
            if (address + availableBytes >= SizeInBytes) availableBytes = SizeInBytes - address; // if this read overflows the end of memory, we can read only this many bytes
            
            IMemorySegment segment = _map.SegmentFor(address);
            if (segment == null) return new byte[numberOfBytes]; // if memory is not allocated return all 0x00s

            if (noTiming && (segment.SizeInBytes - AddressOffset(address, segment)) <= numberOfBytes)
            {
                // if the read fits entirely within the memory segment and we aren't generating read timing, then optimise for speed
                return segment.ReadBytesAt(AddressOffset(address, segment), numberOfBytes);
            }
            else
            {
                byte[] bytes = new byte[numberOfBytes];
                for (int i = 0; i < availableBytes; i++)
                {
                    bytes[i] = ReadByteAt((ushort)(address + i), noTiming);
                }
                return bytes; // bytes beyond the available byte limit (if any) will be 0x00
            }
        }

        public ushort ReadWordAt(ushort address, bool noTiming)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            byte low = ReadByteAt(address, noTiming);
            byte high = ReadByteAt(++address, noTiming);
            return (ushort)((high * 256) + low);
        }

        public void WriteByteAt(ushort address, byte value, bool noTiming)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            IMemorySegment segment = _map.SegmentFor(address);
            if (segment != null || !segment.ReadOnly)
            {
                segment.WriteByteAt(AddressOffset(address, segment), value);
            }
            
            if (!noTiming) _cpu.Cycle.MemoryWriteCycle(AddressOffset(address, segment), value);
        }

        public void WriteBytesAt(ushort address, byte[] bytes, bool noTiming)
        {
            // similar optimisation to ReadBytesAt above
            if (noTiming)
            {
                if (!_initialised) throw new MemoryNotInitialisedException();

                IMemorySegment segment = _map.SegmentFor(address);
                if (segment != null || !segment.ReadOnly)
                {
                    if (segment.SizeInBytes - AddressOffset(address, segment) <= bytes.Length)
                    {
                        segment.WriteBytesAt(AddressOffset(address, segment), bytes);
                    }
                    else
                    {
                        for (ushort i = 0; i < bytes.Length; i++)
                        {
                            WriteByteAt((ushort)(address + i), bytes[i], false);
                        }
                    }
                }
            }
            else
            {
                for (ushort i = 0; i < bytes.Length; i++)
                {
                    WriteByteAt((ushort)(address + i), bytes[i], false);
                }
            }
        }

        public void WriteWordAt(ushort address, ushort value, bool noTiming)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            WriteByteAt(address, (byte)(value % 256), noTiming);
            WriteByteAt(++address, (byte)(value / 256), noTiming);
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

        private ushort AddressOffset(ushort address, IMemorySegment segment)
        {
            return (ushort)(address - segment.StartAddress);
        }

        public MemoryBank()
        {
        }
    }
}

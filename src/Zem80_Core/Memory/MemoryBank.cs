using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace Zem80.Core.Memory
{
    public class MemoryBank : IUntimedMemory, ITimedMemory
    {
        protected IMemoryMap _map;
        protected Processor _cpu;
        protected bool _initialised;

        public ITimedMemory Timed => this;
        public IUntimedMemory Untimed => this;

        public uint SizeInBytes => _map.SizeInBytes;

        byte IUntimedMemory.ReadByteAt(ushort address)
        {
            return ReadByteAt(address, false);
        }

        byte[] IUntimedMemory.ReadBytesAt(ushort address, ushort numberOfBytes)
        {
            return ReadBytesAt(address, numberOfBytes, false);
        }

        ushort IUntimedMemory.ReadWordAt(ushort address)
        {
            return ReadWordAt(address, false);
        }

        void IUntimedMemory.WriteByteAt(ushort address, byte value)
        {
            WriteByteAt(address, value, false);
        }

        void IUntimedMemory.WriteBytesAt(ushort address, byte[] bytes)
        {
            WriteBytesAt(address, bytes, false);
        }

        void IUntimedMemory.WriteWordAt(ushort address, ushort value)
        {
            WriteWordAt(address, value, false);
        }

        byte ITimedMemory.ReadByteAt(ushort address)
        {
            return ReadByteAt(address, true);
        }

        byte[] ITimedMemory.ReadBytesAt(ushort address, ushort numberOfBytes)
        {
            return ReadBytesAt(address, numberOfBytes, true);
        }

        ushort ITimedMemory.ReadWordAt(ushort address)
        {
            return ReadWordAt(address, true);
        }

        void ITimedMemory.WriteByteAt(ushort address, byte value)
        {
            WriteByteAt(address, value, true);
        }

        void ITimedMemory.WriteBytesAt(ushort address, byte[] bytes)
        {
            WriteBytesAt(address, bytes, true);
        }

        void ITimedMemory.WriteWordAt(ushort address, ushort value)
        {
            WriteWordAt(address, value, true);
        }

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

        protected byte ReadByteAt(ushort address, bool timing)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            IMemorySegment segment = _map.SegmentFor(address);
            byte output = (segment?.ReadByteAt(AddressOffset(address, segment)) ?? 0x00); // 0x00 if address is unallocated
            if (timing) _cpu.InstructionTiming.MemoryReadCycle(address, output);
            return output;
        }

        protected byte[] ReadBytesAt(ushort address, ushort numberOfBytes, bool timing)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            uint availableBytes = numberOfBytes;
            if (address + availableBytes >= SizeInBytes) availableBytes = SizeInBytes - address; // if this read overflows the end of memory, we can read only this many bytes

            IMemorySegment segment = _map.SegmentFor(address);
            if (segment == null) return new byte[numberOfBytes]; // if memory is not allocated return all 0x00s

            if (!timing && segment.SizeInBytes - AddressOffset(address, segment) >= numberOfBytes)
            {
                // if the read fits entirely within the memory segment and we aren't generating read timing, then optimise for speed
                return segment.ReadBytesAt(AddressOffset(address, segment), numberOfBytes);
            }
            else
            {
                byte[] bytes = new byte[numberOfBytes];
                for (int i = 0; i < availableBytes; i++)
                {
                    bytes[i] = ReadByteAt((ushort)(address + i), timing);
                }
                return bytes; // bytes beyond the available byte limit (if any) will be 0x00
            }
        }

        protected ushort ReadWordAt(ushort address, bool timing)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            byte low = ReadByteAt(address, timing);
            byte high = ReadByteAt(++address, timing);
            return (ushort)((high * 256) + low);
        }

        protected void WriteByteAt(ushort address, byte value, bool timing)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            IMemorySegment segment = _map.SegmentFor(address);
            if (segment != null || !segment.ReadOnly)
            {
                segment.WriteByteAt(AddressOffset(address, segment), value);
            }

            if (timing) _cpu.InstructionTiming.MemoryWriteCycle(address, value);
        }

        protected void WriteBytesAt(ushort address, byte[] bytes, bool timing)
        {
            // similar optimisation to ReadBytesAt above
            if (!_initialised) throw new MemoryNotInitialisedException();

            IMemorySegment segment = _map.SegmentFor(address);
            if (segment != null || !segment.ReadOnly)
            {
                if (!timing && segment.SizeInBytes - AddressOffset(address, segment) >= bytes.Length)
                {
                    segment.WriteBytesAt(AddressOffset(address, segment), bytes);
                }
                else
                {
                    for (ushort i = 0; i < bytes.Length; i++)
                    {
                        WriteByteAt((ushort)(address + i), bytes[i], timing);
                    }
                }
            }
        }

        protected void WriteWordAt(ushort address, ushort value, bool timing)
        {
            if (!_initialised) throw new MemoryNotInitialisedException();

            WriteByteAt(address, (byte)(value % 256), timing);
            WriteByteAt(++address, (byte)(value / 256), timing);
        }


        protected ushort AddressOffset(ushort address, IMemorySegment segment)
        {
            return (ushort)(address - segment.StartAddress);
        }

        public MemoryBank()
        {
        }
    }
}

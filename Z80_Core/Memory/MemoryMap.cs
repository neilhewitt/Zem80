using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class MemoryMap : IMemoryMap
    {
        public const ushort PAGE_SIZE_IN_BYTES = 1024;

        private Dictionary<ushort, IMemorySegment> _segments = new Dictionary<ushort, IMemorySegment>();

        public uint SizeInBytes { get; private set; }

        public IMemorySegment MemoryFor(ushort address)
        {
            _segments.TryGetValue(PageFromAddress(address), out IMemorySegment segmentForPage);
            return segmentForPage;
        }

        public void Map(IMemorySegment entry, bool overwriteMappedPages = false)
        {
            ushort startAddress = entry.StartAddress;
            uint size = entry.SizeInBytes;

            if (startAddress % 1024 > 0)
            {
                throw new MemoryMapException("Start address must be on a page boundary (divisible by 1024).");
            }

            if (startAddress + size > SizeInBytes)
            {
                throw new MemoryMapException("Block would overflow the memory space which is " + SizeInBytes + " bytes.");
            }

            ushort startPage = PageFromAddress(startAddress);
            ushort endPage = (ushort)(startPage + (size / PAGE_SIZE_IN_BYTES) - 1);

            if (!overwriteMappedPages && _segments.Any(p => p.Key >= startPage && p.Key <= endPage))
            {
                throw new MemoryMapException("Would overwrite existing mapped page/s. Specify overwriteMappedPages = true to enable masking the existing memory."); 
            }

            for (ushort i = startPage; i <= endPage; i++)
            {
                if (_segments.ContainsKey(i))
                {
                    _segments[i] = entry;
                }
                else
                {
                    _segments.Add(i, entry);
                }
            }
        }

        private ushort PageFromAddress(ushort address)
        {
            ushort pageIndex = (ushort)Math.Floor((double)address / (double)(PAGE_SIZE_IN_BYTES));
            return pageIndex;
        }

        private ushort AddressFromPage(ushort pageNumber)
        {
            return (ushort)(pageNumber * PAGE_SIZE_IN_BYTES);
        }

        public MemoryMap(uint sizeInBytes)
        {
            SizeInBytes = sizeInBytes;
        }
    }
}

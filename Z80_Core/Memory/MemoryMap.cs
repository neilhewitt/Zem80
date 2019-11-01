using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class MemoryMap : IMemoryMap
    {
        public const int PAGE_SIZE_IN_BYTES = 1024;

        private Dictionary<int, IMemorySegment> _segments = new Dictionary<int, IMemorySegment>();

        public int SizeInBytes { get; private set; }

        public IMemorySegment MemoryFor(ushort address)
        {
            _segments.TryGetValue(PageFromAddress(address), out IMemorySegment segmentForPage);
            return segmentForPage;
        }

        public void Map(IMemorySegment entry, bool overwriteMappedPages = false)
        {
            ushort startAddress = entry.StartAddress;
            int size = entry.SizeInBytes;

            if (startAddress % 1024 > 0)
            {
                throw new MemoryMapException("Start address must be on a page boundary (divisible by 1024).");
            }

            if (startAddress + size > SizeInBytes)
            {
                throw new MemoryMapException("Block would overflow the memory space which is " + SizeInBytes + " bytes.");
            }

            int startPage = PageFromAddress(startAddress);
            int endPage = startPage + (size / PAGE_SIZE_IN_BYTES) - 1;

            if (!overwriteMappedPages && _segments.Any(p => p.Key >= startPage && p.Key <= endPage))
            {
                throw new MemoryMapException("Would overwrite existing mapped page/s. Specify overwriteMappedPages = true to enable masking the existing memory."); 
            }

            for (int i = startPage; i <= endPage; i++)
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

        private int PageFromAddress(ushort address)
        {
            int pageIndex = (int)Math.Floor((double)address / (double)(PAGE_SIZE_IN_BYTES));
            return pageIndex;
        }

        private int AddressFromPage(ushort pageNumber)
        {
            return pageNumber * PAGE_SIZE_IN_BYTES;
        }

        public MemoryMap(int sizeInBytes)
        {
            SizeInBytes = sizeInBytes;
        }
    }
}

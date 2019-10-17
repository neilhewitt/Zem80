using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class MemoryMap : IMemoryMap
    {
        public const ushort PAGE_SIZE_IN_KILOBYTES = 1;

        private Dictionary<uint, IMemorySegment> _segments = new Dictionary<uint, IMemorySegment>();

        public uint SizeInKilobytes { get; private set; }

        public IMemorySegment MemoryFor(uint address)
        {
            _segments.TryGetValue(PageFromAddress(address), out IMemorySegment segmentForPage);
            return segmentForPage;
        }

        public void Map(IMemorySegment entry, bool overwriteMappedPages = false)
        {
            uint startAddress = entry.StartAddress;
            uint sizeInKilobytes = entry.SizeInKilobytes;

            if (startAddress % 1024 > 0)
            {
                throw new MemoryMapException("Start address must be on a page boundary (divisible by 1024).");
            }

            uint startPage = PageFromAddress(startAddress);
            uint endPage = startPage + sizeInKilobytes - 1;

            if (!overwriteMappedPages && _segments.Any(p => p.Key >= startPage && p.Key <= endPage))
            {
                throw new MemoryMapException("Would overwrite existing mapped page/s. Specify overwriteMappedPages = true to enable masking the existing memory."); 
            }

            for (uint i = startPage; i <= endPage; i++)
            {
                uint address = AddressFromPage(i);
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

        private uint PageFromAddress(uint address)
        {
            uint pageIndex = (uint)Math.Ceiling((double)address / (double)(PAGE_SIZE_IN_KILOBYTES * 1024));
            return pageIndex;
        }

        private uint AddressFromPage(uint pageNumber)
        {
            return (uint)(pageNumber * PAGE_SIZE_IN_KILOBYTES);
        }

        public MemoryMap(uint sizeInKilobytes)
        {
            SizeInKilobytes = sizeInKilobytes;
        }
    }
}

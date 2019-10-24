using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class MemoryMap : IMemoryMap
    {
        public const ushort PAGE_SIZE_IN_KILOBYTES = 1;

        private Dictionary<ushort, IMemorySegment> _segments = new Dictionary<ushort, IMemorySegment>();

        public ushort SizeInKilobytes { get; private set; }

        public IMemorySegment MemoryFor(ushort address)
        {
            _segments.TryGetValue(PageFromAddress(address), out IMemorySegment segmentForPage);
            return segmentForPage;
        }

        public void Map(IMemorySegment entry, bool overwriteMappedPages = false)
        {
            ushort startAddress = entry.StartAddress;
            ushort sizeInKilobytes = entry.SizeInKilobytes;

            if (startAddress % 1024 > 0)
            {
                throw new MemoryMapException("Start address must be on a page boundary (divisible by 1024).");
            }

            ushort startPage = PageFromAddress(startAddress);
            ushort endPage = (ushort)(startPage + sizeInKilobytes - 1);

            if (!overwriteMappedPages && _segments.Any(p => p.Key >= startPage && p.Key <= endPage))
            {
                throw new MemoryMapException("Would overwrite existing mapped page/s. Specify overwriteMappedPages = true to enable masking the existing memory."); 
            }

            for (ushort i = startPage; i <= endPage; i++)
            {
                ushort address = AddressFromPage(i);
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
            ushort pageIndex = (ushort)Math.Ceiling((double)address / (double)(PAGE_SIZE_IN_KILOBYTES * 1024));
            return pageIndex;
        }

        private ushort AddressFromPage(ushort pageNumber)
        {
            return (ushort)(pageNumber * PAGE_SIZE_IN_KILOBYTES);
        }

        public MemoryMap(ushort sizeInKilobytes)
        {
            SizeInKilobytes = sizeInKilobytes;
        }
    }
}

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
                throw new Exception("Start address must be on a page boundary (divisible by 1024)."); // TODO: custom exception
            }

            uint startPage = PageFromAddress(startAddress);
            uint endPage = startPage + sizeInKilobytes - 1;

            if (!overwriteMappedPages && _segments.Any(p => p.Key >= startPage && p.Key <= endPage))
            {
                throw new Exception("Would overwrite existing mapped page. Pass overwriteMappedPages = true to enable."); // TODO: custom exception
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
            return (uint)(address / PAGE_SIZE_IN_KILOBYTES);
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

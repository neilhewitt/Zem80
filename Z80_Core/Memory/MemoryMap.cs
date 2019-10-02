using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class MemoryMap : IMemoryMap
    {
        public const ushort PAGE_SIZE_IN_KILOBYTES = 1;

        private Dictionary<uint, IMemoryLocation> _pages = new Dictionary<uint, IMemoryLocation>();

        public uint SizeInKilobytes { get; private set; }

        public IMemoryLocation MemoryFor(uint address)
        {
            _pages.TryGetValue(PageNumberFromAddress(address), out IMemoryLocation memoryForPage);
            return memoryForPage;
        }

        public void Map(uint startAddress, uint sizeInKilobytes, IMemoryLocation memory, bool overwriteMappedPages = false)
        {
            if (startAddress % 1024 > 0)
            {
                throw new Exception("Start address must be on a page boundary (divisible by 1024)."); // TODO: custom exception
            }

            uint startPage = PageNumberFromAddress(startAddress);
            uint endPage = startPage + sizeInKilobytes - 1;

            if (!overwriteMappedPages && _pages.Any(p => p.Key >= startPage && p.Key <= endPage))
            {
                throw new Exception("Would overwrite existing mapped page. Pass overwriteMappedPages = true to enable."); // TODO: custom exception
            }

            for (uint i = startPage; i <= endPage; i++)
            {
                uint address = AddressFromPageNumber(i);
                if (_pages.ContainsKey(i))
                {
                    _pages[i] = memory;
                }
                else
                {
                    _pages.Add(i, memory);
                }
            }
        }

        private uint PageNumberFromAddress(uint address)
        {
            return (uint)(address / PAGE_SIZE_IN_KILOBYTES);
        }

        private uint AddressFromPageNumber(uint pageNumber)
        {
            return (uint)(pageNumber * PAGE_SIZE_IN_KILOBYTES);
        }

        public MemoryMap(uint sizeInKilobytes = 64)
        {
            SizeInKilobytes = sizeInKilobytes;
        }
    }
}

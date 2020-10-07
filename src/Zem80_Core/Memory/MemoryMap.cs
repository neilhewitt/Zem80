using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Zem80.Core.Memory
{
    public class MemoryMap : IMemoryMap
    {
        public const int PAGE_SIZE_IN_BYTES = 1024;

        private IMemorySegment[] _pageMap;

        private List<IMemorySegment> _segments = new List<IMemorySegment>();

        public uint SizeInBytes { get; private set; }

        public IMemorySegment MemoryFor(ushort address)
        {
            return _pageMap[PageFromAddress(address)];
        }

        public void Map(IMemorySegment entry, bool overwriteMappedPages = false)
        {
            int size = (int)entry.SizeInBytes;
            ushort startAddress = entry.StartAddress;

            if (startAddress % PAGE_SIZE_IN_BYTES > 0)
            {
                throw new MemoryMapException($"Start address must be on a page boundary (divisible by {PAGE_SIZE_IN_BYTES}).");
            }

            if (startAddress + size > SizeInBytes)
            {
                throw new MemoryMapException("Block would overflow the memory space which is " + SizeInBytes + " bytes.");
            }

            int startPage = PageFromAddress(startAddress);
            int endPage = startPage + (size / PAGE_SIZE_IN_BYTES) - 1;

            if (!overwriteMappedPages && _pageMap[startPage..endPage].Any(p => p!= null))
            {
                throw new MemoryMapException("Would overwrite existing mapped page/s. Specify overwriteMappedPages = true to enable masking the existing memory."); 
            }

            if (!_segments.Contains(entry))
            {
                _segments.Add(entry);
            }

            for (int i = startPage; i <= endPage; i++)
            {
                _pageMap[i] = entry;
            }
        }

        public void ClearAllWritableMemory()
        {
            foreach(IMemorySegment segment in _segments)
            {
                segment.Clear();
            }
        }

        private int PageFromAddress(ushort address)
        {
            int pageIndex = (address / PAGE_SIZE_IN_BYTES);
            return pageIndex;
        }
        public MemoryMap(uint sizeInBytes, bool autoMap = false)
        {
            SizeInBytes = sizeInBytes;
            int pages = (int)(sizeInBytes / PAGE_SIZE_IN_BYTES);
            _pageMap = new IMemorySegment[pages];

            if (autoMap) Map(new MemorySegment(0, sizeInBytes)); // maps a single block to the whole of memory space (you can map ROM in later)
        }
    }
}

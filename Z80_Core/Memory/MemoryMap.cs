﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class MemoryMap : IMemoryMap
    {
        public const int PAGE_SIZE_IN_BYTES = 1024;

        private Dictionary<int, IMemorySegment> _segmentMap = new Dictionary<int, IMemorySegment>();
        private List<IMemorySegment> _segments = new List<IMemorySegment>();

        public int SizeInBytes { get; private set; }

        public IMemorySegment MemoryFor(ushort address)
        {
            _segmentMap.TryGetValue(PageFromAddress(address), out IMemorySegment segmentForPage);
            return segmentForPage;
        }

        public void Map(IMemorySegment entry, ushort startAddress, bool overwriteMappedPages = false)
        {
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

            if (!overwriteMappedPages && _segmentMap.Any(p => p.Key >= startPage && p.Key <= endPage))
            {
                throw new MemoryMapException("Would overwrite existing mapped page/s. Specify overwriteMappedPages = true to enable masking the existing memory."); 
            }

            if (!_segments.Contains(entry))
            {
                _segments.Add(entry);
            }

            for (int i = startPage; i <= endPage; i++)
            {
                if (_segmentMap.ContainsKey(i))
                {
                    _segmentMap[i] = entry;
                }
                else
                {
                    _segmentMap.Add(i, entry);
                }
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
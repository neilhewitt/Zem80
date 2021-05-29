using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Zem80.Core.Memory
{
    public class MemoryMap : IMemoryMap
    {
        /* 
         * This class provides a memory mapper for setting up the memory for the emulator. Memory can be mapped into the address space in segments across pages of 1K in size. 
         * Segments can be read/write or readonly, and must be between 1024 and 65536 bytes in size, and should be of a size that is divisible by 1024 (ie fits on a page boundary). 
         * Note that if you supply a memory segment that is smaller than the total number of pages covered * 1024 bytes, the missing bytes will always be read as 0x00 and any 
         * writes will be ignored. In a normal scenario the maximum possible memory size for the Z80 is 64K or 65536 bytes, usually a combination of ROM and RAM. 
         * 
         * The memory system has been designed with expansion in mind. All the memory types are handled by the rest of the emulator as interfaces, so you can provide your own 
         * implementation of the memory map, memory bank, and memory segments. You could, for example, add a paged memory system such as that used by the ZX Spectrum 128.
         * You can supply your own populated memory map via the interface type in the constructor for the Processor class. This allows you to completely replace the memory
         * implementation if you wish. 
         */

        public const int PAGE_SIZE_IN_BYTES = 1024;
        private const uint MAX_MEMORY_MAP_SIZE = 65536;

        private IMemorySegment[] _pageMap;

        private List<IMemorySegment> _segments = new List<IMemorySegment>();

        public uint SizeInBytes { get; private set; }

        public IMemorySegment SegmentFor(ushort address)
        {
            return _pageMap[PageFromAddress(address)];
        }

        public void Map(IMemorySegment segment, ushort startAddress, bool overwriteMappedPages = false)
        {
            int size = (int)segment.SizeInBytes;

            if (startAddress % PAGE_SIZE_IN_BYTES > 0)
            {
                throw new MemoryMapException($"Start address must be on a page boundary (divisible by {PAGE_SIZE_IN_BYTES}).");
            }

            if (startAddress + size > SizeInBytes)
            {
                throw new MemoryMapException($"Block would overflow the memory space which is {SizeInBytes} bytes.");
            }

            int startPage = PageFromAddress(startAddress);
            int endPage = startPage + (size / PAGE_SIZE_IN_BYTES) - 1;

            if (!overwriteMappedPages && _pageMap[startPage..endPage].Any(p => p!= null))
            {
                throw new MemoryMapException("Would overwrite existing mapped page/s. Specify overwriteMappedPages = true to enable masking the existing memory."); 
            }

            if (!_segments.Contains(segment))
            {
                _segments.Add(segment);
                segment.MapAt(startAddress);
            }
            else
            {
                throw new MemoryMapException("This segment has already been mapped into memory and cannot be mapped twice.");
            }

            for (int i = startPage; i <= endPage; i++)
            {
                _pageMap[i] = segment; // map a reference to this segment into every 1K page it runs over, so we can quickly fetch the segment for any address
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
            if (sizeInBytes > MAX_MEMORY_MAP_SIZE)
            {
                throw new MemoryMapException("Requested memory map size exceeds the maximum which is " + MAX_MEMORY_MAP_SIZE.ToString() + " bytes.");
            }

            SizeInBytes = sizeInBytes;
            int pages = (int)(sizeInBytes / PAGE_SIZE_IN_BYTES);
            _pageMap = new IMemorySegment[pages];

            if (autoMap) Map(new MemorySegment(sizeInBytes), 0); // maps a single block to the whole of memory space (you can map ROM in later)
        }
    }
}

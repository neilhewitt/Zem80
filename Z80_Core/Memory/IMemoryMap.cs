namespace Z80.Core
{
    public interface IMemoryMap
    {
        uint SizeInKilobytes { get; }

        void Map(IMemorySegment memory, bool overwriteMappedPages = false);
        IMemorySegment MemoryFor(uint address);
    }
}
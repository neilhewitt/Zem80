namespace Z80.Core
{
    public interface IMemoryMap
    {
        uint SizeInKilobytes { get; }

        void Map(uint startAddress, uint sizeInKilobytes, IMemoryLocation memory, bool overwriteMappedPages = false);
        IMemoryLocation MemoryFor(uint address);
    }
}
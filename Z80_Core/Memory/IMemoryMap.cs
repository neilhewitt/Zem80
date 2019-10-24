namespace Z80.Core
{
    public interface IMemoryMap
    {
        ushort SizeInKilobytes { get; }

        void Map(IMemorySegment memory, bool overwriteMappedPages = false);
        IMemorySegment MemoryFor(ushort address);
    }
}
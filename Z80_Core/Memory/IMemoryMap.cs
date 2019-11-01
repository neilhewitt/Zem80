namespace Z80.Core
{
    public interface IMemoryMap
    {
        int SizeInBytes { get; }

        void Map(IMemorySegment memory, bool overwriteMappedPages = false);
        IMemorySegment MemoryFor(ushort address);
    }
}
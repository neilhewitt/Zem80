namespace Zem80.Core.Memory
{
    public interface IMemoryMap
    {
        uint SizeInBytes { get; }

        void Map(IMemorySegment memory, bool overwriteMappedPages = false);
        void ClearAllWritableMemory();
        IMemorySegment MemoryFor(ushort address);
    }
}
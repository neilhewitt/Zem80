namespace Z80.Core
{
    public interface IMemoryMap
    {
        int SizeInBytes { get; }
        ushort TopOfStack { get; }

        void Map(IMemorySegment memory, ushort startAddress, bool overwriteMappedPages = false);
        void ClearAllWritableMemory();
        IMemorySegment MemoryFor(ushort address);
    }
}
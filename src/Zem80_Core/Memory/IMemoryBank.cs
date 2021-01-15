namespace Zem80.Core.Memory
{
    public interface IMemoryBank
    {
        uint SizeInBytes { get; }
        ITimedMemory Timed { get; }
        IUntimedMemory Untimed { get; }

        void Clear();
        void Initialise(Processor cpu, IMemoryMap map);
    }
}
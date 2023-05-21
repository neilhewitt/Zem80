using Zem80.Core.CPU;

namespace Zem80.Core.Memory
{
    public interface IMemoryBank
    {
        uint SizeInBytes { get; }
        IMemory Timed { get; }
        IMemory Untimed { get; }

        void Clear();
        void Initialise(Processor cpu, IMemoryMap map);
    }
}
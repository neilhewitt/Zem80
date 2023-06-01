using Zem80.Core.CPU;
using Zem80.Core.CPU;

namespace Zem80.Core.Memory
{
    public interface IMemoryBank
    {
        uint SizeInBytes { get; }
        IMemory Untimed { get; }
        IMemory Timed { get; }

        void Clear();
        void Initialise(Processor cpu, IMemoryMap map);
    }
}
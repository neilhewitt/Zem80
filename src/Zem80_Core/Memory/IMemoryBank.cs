using Zem80.Core.CPU;
using Zem80.Core.Instructions;

namespace Zem80.Core.Memory
{
    public interface IMemoryBank
    {
        uint SizeInBytes { get; }
        IMemory Untimed { get; }

        IMemory TimedFor(Instruction instruction);

        void Clear();
        void Initialise(Processor cpu, IMemoryMap map);
    }
}
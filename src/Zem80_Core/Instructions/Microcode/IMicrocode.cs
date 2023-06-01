using System;

namespace Zem80.Core.CPU
{
    public interface IMicrocode
    {
        ExecutionResult Execute(Processor cpu, InstructionPackage package);
    }
}
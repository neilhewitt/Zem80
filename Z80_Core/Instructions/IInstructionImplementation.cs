using System;

namespace Z80.Core
{
    public interface IMicrocode
    {
        ExecutionResult Execute(Processor cpu, InstructionPackage package);
    }
}
using System;

namespace Z80.Core
{
    public interface IInstructionImplementation
    {
        ExecutionResult Execute(IProcessor cpu, InstructionPackage package);
    }
}
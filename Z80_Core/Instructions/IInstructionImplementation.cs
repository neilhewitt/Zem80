using System;

namespace Z80.Core
{
    public interface IInstructionImplementation
    {
        ExecutionResult Execute(InstructionPackage package);
    }
}
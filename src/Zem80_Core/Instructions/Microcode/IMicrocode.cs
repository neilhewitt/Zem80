using System;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
{
    public interface IMicrocode
    {
        ExecutionResult Execute(Processor cpu, InstructionPackage package);
    }
}
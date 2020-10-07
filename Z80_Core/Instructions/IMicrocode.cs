using System;

namespace Zem80.Core.Instructions
{
    public interface IMicrocode
    {
        ExecutionResult Execute(Processor cpu, ExecutionPackage package);
    }
}
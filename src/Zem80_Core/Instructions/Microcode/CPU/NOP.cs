using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class NOP : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public NOP()
        {
        }
    }
}

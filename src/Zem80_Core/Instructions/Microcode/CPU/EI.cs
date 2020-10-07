using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class EI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            cpu.EnableInterrupts();
            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public EI()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class DI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            cpu.DisableInterrupts();
            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public DI()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class DI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.Interrupts.Disable();
            return new ExecutionResult(package, null);
        }

        public DI()
        {
        }
    }
}

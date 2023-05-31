using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class EI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.Interrupts.Enable();
            return new ExecutionResult(package, cpu.Flags.Clone()); // interrupts aren't enabled until the *next* instruction
        }

        public EI()
        {
        }
    }
}

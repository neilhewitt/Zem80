using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
{
    public class EI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.EnableInterrupts();
            return new ExecutionResult(package, cpu.Flags.Clone()); // interrupts aren't enabled until the *next* instruction
        }

        public EI()
        {
        }
    }
}

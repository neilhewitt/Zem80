using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class HALT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.Halt(HaltReason.HaltInstruction);

            return new ExecutionResult(package, null);
        }

        public HALT()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class HALT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            cpu.Halt(HaltReason.HaltInstruction);

            return new ExecutionResult(package, null);
        }

        public HALT()
        {
        }
    }
}

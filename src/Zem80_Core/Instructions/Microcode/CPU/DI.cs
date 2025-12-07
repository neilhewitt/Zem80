using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class DI : MicrocodeBase
    {
        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            cpu.Interrupts.Disable();
            return new ExecutionResult(package, null);
        }

        public DI()
        {
        }
    }
}

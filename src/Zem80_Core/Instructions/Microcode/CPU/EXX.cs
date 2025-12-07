using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class EXX : MicrocodeBase
    {
        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            cpu.Registers.ExchangeBCDEHL();
            return new ExecutionResult(package, null);
        }

        public EXX()
        {
        }
    }
}

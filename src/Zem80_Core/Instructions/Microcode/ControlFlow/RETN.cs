using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RETN : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            cpu.Stack.Pop(WordRegister.PC);
            cpu.Registers.WZ = cpu.Registers.PC;
            cpu.Interrupts.RestoreAfterNMI(); // will re-enable maskable interrupts if they were enabled before entering the NMI handler
            return new ExecutionResult(package, null);
        }

        public RETN()
        {
        }
    }
}

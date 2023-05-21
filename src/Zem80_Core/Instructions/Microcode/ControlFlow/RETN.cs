using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
{
    public class RETN : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.Stack.Pop(WordRegister.PC);
            cpu.Registers.WZ = cpu.Registers.PC;
            cpu.RestoreInterruptsAfterNMI(); // will re-enable maskable interrupts if they were enabled before entering the NMI handler
            return new ExecutionResult(package, null);
        }

        public RETN()
        {
        }
    }
}

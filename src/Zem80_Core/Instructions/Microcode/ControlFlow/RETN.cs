using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class RETN : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.Pop(WordRegister.PC);
            cpu.Registers.WZ = cpu.Registers.PC;
            cpu.RestoreInterruptsFromNMI(); // will re-enable maskable interrupts if they were enabled before entering the NMI handler
            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public RETN()
        {
        }
    }
}

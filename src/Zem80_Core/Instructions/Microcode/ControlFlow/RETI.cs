using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class RETI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.Pop(WordRegister.PC);
            cpu.IFF1 = cpu.IFF2; // will re-enable maskable interrupts if they were enabled before entering an NMI handler
            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public RETI()
        {
        }
    }
}

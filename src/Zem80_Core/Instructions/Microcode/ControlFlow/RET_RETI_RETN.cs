using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RET: RET_RETI_RETN { public RET() { } }
    public class RETI: RET_RETI_RETN { public RETI() { } }
    public class RETN: RET_RETI_RETN { public RETN() { } }

    public class RET_RETI_RETN : MicrocodeBase
    {
        // RET
        // RET cc
        // RETI
        // RETN

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            Flags flags = cpu.Flags.Clone();

            if (instruction.Condition == Condition.None || flags.SatisfyCondition(instruction.Condition))
            {
                cpu.Stack.Pop(WordRegister.PC);
                cpu.Registers.WZ = cpu.Registers.PC;
                if (this is RETN)
                {
                    // will re-enable maskable interrupts if they were enabled before entering the NMI handler
                    cpu.Interrupts.RestoreAfterNMI();
                }
            }
            
            return new ExecutionResult(package, flags);
        }

        public RET_RETI_RETN()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RET : IMicrocode
    {
        // RET
        // RET cc

        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            Flags flags = cpu.Flags.Clone();

            if (instruction.Condition == Condition.None || flags.SatisfyCondition(instruction.Condition))
            {
                cpu.Stack.Pop(WordRegister.PC);
                cpu.Registers.WZ = cpu.Registers.PC;
            }
            
            return new ExecutionResult(package, flags);
        }

        public RET()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RET : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            if (instruction.Condition == Condition.None || flags.SatisfyCondition(instruction.Condition))
            {
                cpu.Pop(WordRegister.PC);
            }

            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public RET()
        {
        }
    }
}

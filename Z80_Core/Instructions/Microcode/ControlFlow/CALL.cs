using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CALL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            if (instruction.Condition == Condition.None || flags.SatisfyCondition(instruction.Condition))
            {
                cpu.Push(WordRegister.PC);
                cpu.Registers.PC = data.ArgumentsAsWord;
            }

            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public CALL()
        {
        }
    }
}

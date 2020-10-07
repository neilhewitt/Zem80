using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
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
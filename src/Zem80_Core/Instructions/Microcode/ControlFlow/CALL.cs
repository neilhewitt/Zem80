using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class CALL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();

            if (instruction.Condition == Condition.None || flags.SatisfyCondition(instruction.Condition))
            {
                cpu.Stack.Push(WordRegister.PC);
                cpu.Registers.PC = data.ArgumentsAsWord;
            }

            cpu.Registers.WZ = data.ArgumentsAsWord;

            return new ExecutionResult(package, flags);
        }

        public CALL()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Zem80.Core.CPU
{
    public class JP : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();

            if (instruction.Source != InstructionElement.WordValue)
            {
                // JP HL / IX / IY
                cpu.Registers.PC = instruction.MarshalSourceWord(data, cpu, 0);
            }
            else if (instruction.Condition == Condition.None || flags.SatisfyCondition(instruction.Condition))
            {
                cpu.Registers.PC = data.ArgumentsAsWord;
                cpu.Registers.WZ = data.ArgumentsAsWord;
            }

            return new ExecutionResult(package, flags);
        }

        public JP()
        {
        }
    }
}

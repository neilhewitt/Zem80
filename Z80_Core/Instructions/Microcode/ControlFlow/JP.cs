using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class JP : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            if (instruction.Source != InstructionElement.WordValue)
            {
                // JP HL / IX / IY
                cpu.Registers.PC = instruction.MarshalSourceWord(data, cpu, out ushort address);
            }
            else if (instruction.Condition == Condition.None || flags.SatisfyCondition(instruction.Condition))
            {
                cpu.Registers.PC = data.ArgumentsAsWord;
            }

            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public JP()
        {
        }
    }
}

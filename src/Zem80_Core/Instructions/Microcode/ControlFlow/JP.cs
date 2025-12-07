using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Zem80.Core.CPU
{
    public class JP : IMicrocode
    {
        // JP nn
        // JP cc,nn
        // JP (HL)
        // JP (IX)
        // JP (IY)

        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();

            if (instruction.Source != InstructionElement.WordValue)
            {
                // JP HL / IX / IY
                cpu.Registers.PC = Resolver.GetSourceWord(instruction, data, cpu, 0);
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

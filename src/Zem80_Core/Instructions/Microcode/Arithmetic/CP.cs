using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class CP : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;

            byte left = r.A;
            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            byte right = Resolver.GetSourceByte(instruction, data, cpu, 3);
            (byte result, Flags flags) = Arithmetic.Subtract(left, right, false);
            flags.X = (right & 0x08) > 0; // copy bit 3 of operand, not result
            flags.Y = (right & 0x20) > 0; // copy bit 5 of operand, not result

            return new ExecutionResult(package, flags);
        }

        public CP()
        {
        }
    }
}

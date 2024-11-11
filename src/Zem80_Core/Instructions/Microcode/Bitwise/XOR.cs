using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class XOR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;

            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            byte operand = Resolver.GetSourceByte(instruction, data, cpu, out ushort address, 3);
            (int result, Flags flags) = Logical.Xor(r.A, operand);
            r.A = (byte)result;

            return new ExecutionResult(package, flags);
        }

        public XOR()
        {
        }
    }
}

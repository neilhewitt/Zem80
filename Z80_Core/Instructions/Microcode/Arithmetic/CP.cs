using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CP : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            Flags flags = cpu.Registers.Flags;

            byte left = r.A;
            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            byte right = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);

            var sub = ALUOperations.Subtract(left, right, false);
            flags = sub.Flags;
            flags.X = (right & 0x08) > 0; // copy bit 3 of operand, not result
            flags.Y = (right & 0x20) > 0; // copy bit 5 of operand, not result

            return new ExecutionResult(package, flags);
        }

        public CP()
        {
        }
    }
}

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

            return new ExecutionResult(package, flags);
        }

        public CP()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class XOR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            byte operand = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);
            int result = (r.A ^ operand);
            flags = FlagLookup.LogicalFlags(r.A, operand, LogicalOperation.Xor);
            r.A = (byte)result;

            return new ExecutionResult(package, flags);
        }

        public XOR()
        {
        }
    }
}

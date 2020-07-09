using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class AND : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            Flags flags = cpu.Registers.Flags;
            sbyte offset = (sbyte)data.Argument1;

            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            byte operand = instruction.MarshalSourceByte(data, cpu, out ushort address);
            int result = (r.A & operand);
            flags = FlagLookup.LogicalFlags(r.A, operand, LogicalOperation.And);
            r.A = (byte)result;            

            return new ExecutionResult(package, flags);
        }

        public AND()
        {
        }
    }
}

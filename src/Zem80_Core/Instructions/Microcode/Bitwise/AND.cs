using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class AND : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            Flags flags = cpu.Flags.Clone();
            sbyte offset = (sbyte)data.Argument1;

            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            byte operand = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);
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

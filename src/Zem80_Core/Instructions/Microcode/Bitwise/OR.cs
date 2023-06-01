using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class OR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;

            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            byte operand = instruction.MarshalSourceByte(data, cpu);
            int result = (r.A | operand);
            Flags flags = FlagLookup.LogicalFlags(r.A, operand, LogicalOperation.Or);
            r.A = (byte)result;

            return new ExecutionResult(package, flags);
        }

        public OR()
        {
        }
    }
}

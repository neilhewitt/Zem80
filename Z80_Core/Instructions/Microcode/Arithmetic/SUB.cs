using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SUB : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            byte left = r.A;
            byte right = instruction.MarshalSourceByte(data, cpu, out ushort address);

            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            var sub = ALUOperations.Subtract(left, right, false);
            r.A = sub.Result;
            flags = sub.Flags;

            return new ExecutionResult(package, flags);
        }

        public SUB()
        {
        }
    }
}

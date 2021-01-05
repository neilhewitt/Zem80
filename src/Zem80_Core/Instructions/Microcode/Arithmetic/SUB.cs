using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class SUB : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            byte left = r.A;
            byte right = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);

            if (instruction.IsIndexed) cpu.InstructionTiming.InternalOperationCycle(5);
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

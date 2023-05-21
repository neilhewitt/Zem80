using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
{
    public class SUB : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;

            byte left = r.A;
            byte right = instruction.MarshalSourceByte(data, cpu);

            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            var sub = ALUOperations.Subtract(left, right, false);
            r.A = sub.Result;

            return new ExecutionResult(package, sub.Flags);
        }

        public SUB()
        {
        }
    }
}

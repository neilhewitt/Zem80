using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class SUB : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;

            byte left = r.A;
            byte right = instruction.MarshalSourceByte(data, cpu, 3);

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

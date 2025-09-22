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
            Flags flags;

            byte left = r.A;
            byte right = Resolver.GetSourceByte(instruction, data, cpu, 3);
            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            (r.A, flags) = Arithmetic.Subtract(left, right, false);

            return new ExecutionResult(package, flags);
        }

        public SUB()
        {
        }
    }
}

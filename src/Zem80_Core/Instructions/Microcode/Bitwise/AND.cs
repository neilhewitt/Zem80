using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class AND : IMicrocode
    {
        // AND r
        // AND n
        // AND (HL)
        // AND (IX+o)
        // AND (IY+o)

        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;

            if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
            byte operand = Resolver.GetSourceByte(instruction, data, cpu, 3);
            (int result, Flags flags) = Logical.And(r.A, operand);
            r.A = (byte)result;            

            return new ExecutionResult(package, flags);
        }

        public AND()
        {
        }
    }
}

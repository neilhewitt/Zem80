using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class SBC : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            if (instruction.TargetsWordRegister)
            {
                ushort left = r.HL;
                cpu.Timing.InternalOperationCycle(4);
                cpu.Timing.InternalOperationCycle(3);

                ushort right = instruction.MarshalSourceWord(data, cpu, out ushort address);

                var subtraction = ALUOperations.Subtract(left, right, flags.Carry, true, flags);
                r.HL = subtraction.Result;
                flags = subtraction.Flags;
            }
            else
            {
                byte left = r.A;
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                byte right = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);

                var subtraction = ALUOperations.Subtract(left, right, flags.Carry);
                r.A = subtraction.Result;
                flags = subtraction.Flags;
            }

            return new ExecutionResult(package, flags);
        }

        public SBC()
        {
        }
    }
}

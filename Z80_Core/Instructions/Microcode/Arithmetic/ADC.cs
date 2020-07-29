using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ADC : IMicrocode
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

                var addition = ALUOperations.Add(left, right, flags.Carry, true, flags);
                r.HL = addition.Result;
                flags = addition.Flags;
            }
            else
            {
                byte left = r.A;
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                byte right = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);

                var addition = ALUOperations.Add(left, right, flags.Carry);
                r.A = addition.Result;
                flags = addition.Flags;
            }
          
            return new ExecutionResult(package, flags);
        }

        public ADC()
        {
        }
    }
}

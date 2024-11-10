using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class ADC : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            if (instruction.TargetsWordRegister)
            {
                ushort left = r.HL;
                cpu.Timing.InternalOperationCycle(4);
                cpu.Timing.InternalOperationCycle(3);
                ushort right = instruction.MarshalSourceWord(data, cpu, 3);

                var addition = ArithmeticOperations.Add(left, right, flags.Carry, true, flags);
                r.HL = addition.Result;
                flags = addition.Flags;
                r.WZ = (ushort)(left + 1);
            }
            else
            {
                byte left = r.A;
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                byte right = instruction.MarshalSourceByte(data, cpu, 3);

                var addition = ArithmeticOperations.Add(left, right, flags.Carry);
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

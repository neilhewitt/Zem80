using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Zem80.Core.CPU
{
    public class ADD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            if (instruction.TargetsWordRegister)
            {
                // it's one of the 16-bit adds (HL,DE etc)
                WordRegister destination = instruction.Target.AsWordRegister();
                ushort left = r[destination];
                ushort right = instruction.MarshalSourceWord(data, cpu, 3);

                cpu.Timing.InternalOperationCycle(4);
                cpu.Timing.InternalOperationCycle(3);
                var sum = ArithmeticOperations.Add(left, right, false, false, flags);
                r[destination] = sum.Result;
                flags = sum.Flags;
                r.WZ = (ushort)(left + 1);
            }
            else
            {
                // it's an 8-bit add to A
                byte left = r.A;
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                byte right = instruction.MarshalSourceByte(data, cpu, 3);

                var sum = ArithmeticOperations.Add(left, right, false);
                r.A = sum.Result;
                flags = sum.Flags;
            }

            return new ExecutionResult(package, flags);
        }

        public ADD()
        {
        }
    }
}

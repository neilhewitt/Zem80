using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Zem80.Core.CPU
{
    public class ADD : IMicrocode
    {
        // ADD A,r
        // ADD A,n
        // ADD A,(HL)
        // ADD A,(IX+o)
        // ADD A,(IY+o)
        // ADD HL,rr
        // ADD IX,rr
        // ADD IY,rr

        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
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
                cpu.Timing.InternalOperationCycles(4, 3);
                ushort right = r[instruction.Source.AsWordRegister()];
                (r[destination], flags) = Arithmetic.Add(left, right, false, false, flags);
                r.WZ = (ushort)(left + 1);
            }
            else
            {
                // it's an 8-bit add to A
                byte left = r.A;
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                byte right = Resolver.GetSourceByte(instruction, data, cpu, 3);
                (r.A, flags) = Arithmetic.Add(left, right, false);
            }

            return new ExecutionResult(package, flags);
        }

        public ADD()
        {
        }
    }
}

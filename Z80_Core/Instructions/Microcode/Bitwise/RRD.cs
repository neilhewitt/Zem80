using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RRD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            byte xHL = cpu.Memory.ReadByteAt(cpu.Registers.HL);
            byte a = cpu.Registers.A;

            // result = (HL) = LO: high-order bits of (HL) + HI: low-order bits of A
            // A = LO: low-order bits of (HL) + HI: high-order bits of A

            bool[] lowA = a.GetLowNybble();
            a = a.SetLowNybble(xHL.GetLowNybble());
            xHL = xHL.SetLowNybble(xHL.GetHighNybble());
            xHL = xHL.SetHighNybble(lowA);

            cpu.InternalOperationCycle(4);
            cpu.Memory.WriteByteAt(cpu.Registers.HL, xHL);
            cpu.Registers.A = a;

            flags = FlagLookup.BitwiseFlags(a, BitwiseOperation.RotateRight);
            flags.HalfCarry = false;
            flags.Subtract = false;

            return new ExecutionResult(package, cpu.Registers.Flags, false, false);
        }

        public RRD()
        {
        }
    }
}

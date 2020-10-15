using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class RRD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            byte xHL = cpu.Memory.ReadByteAt(cpu.Registers.HL, false);
            byte a = cpu.Registers.A;

            // result = (HL) = LO: high-order bits of (HL) + HI: low-order bits of A
            // A = LO: low-order bits of (HL) + HI: high-order bits of A

            bool[] lowA = a.GetLowNybble();
            a = a.SetLowNybble(xHL.GetLowNybble());
            xHL = xHL.SetLowNybble(xHL.GetHighNybble());
            xHL = xHL.SetHighNybble(lowA);

            cpu.Timing.InternalOperationCycle(4);
            cpu.Memory.WriteByteAt(cpu.Registers.HL, xHL, false);
            cpu.Registers.A = a;

            // bitwise flag lookup doesn't work for this instruction
            flags.Sign = ((sbyte)a < 0);
            flags.Zero = (a == 0);
            flags.ParityOverflow = a.EvenParity();
            flags.HalfCarry = false;
            flags.Subtract = false;
            flags.X = (a & 0x08) > 0; // copy bit 3
            flags.Y = (a & 0x20) > 0; // copy bit 5

            // leave carry alone

            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public RRD()
        {
        }
    }
}

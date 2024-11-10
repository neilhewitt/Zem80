using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RRD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = cpu.Flags.Clone();

            byte xHL = cpu.Memory.ReadByteAt(cpu.Registers.HL, 4);
            byte A = cpu.Registers.A;

            // result = (HL) = LO: high-order bits of (HL) + HI: low-order bits of A
            // A = LO: low-order bits of (HL) + HI: high-order bits of A

            byte lowA = A.GetLowNybble();
            A = A.SetLowNybble(xHL.GetLowNybble());
            xHL = xHL.SetLowNybble(xHL.GetHighNybble());
            xHL = xHL.SetHighNybble(lowA);

            cpu.Timing.InternalOperationCycle(4);
            cpu.Memory.WriteByteAt(cpu.Registers.HL, xHL, 3);
            cpu.Registers.A = A;

            // bitwise flag lookup doesn't work for this instruction
            flags.Sign = ((sbyte)A < 0);
            flags.Zero = (A == 0);
            flags.ParityOverflow = A.EvenParity();
            flags.HalfCarry = false;
            flags.Subtract = false;
            flags.X = (A & 0x08) > 0; // copy bit 3
            flags.Y = (A & 0x20) > 0; // copy bit 5

            // leave carry alone

            cpu.Registers.WZ = (ushort)(cpu.Registers.HL + 1);

            return new ExecutionResult(package, flags);
        }

        public RRD()
        {
        }
    }
}

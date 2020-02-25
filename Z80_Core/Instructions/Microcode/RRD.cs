using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RRD : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();

            byte xHL = cpu.Memory.ReadByteAt(cpu.Registers.HL);
            byte a = cpu.Registers.A;

            // result = (HL) = LO: high-order bits of (HL) + HI: low-order bits of A
            // A = LO: low-order bits of (HL) + HI: high-order bits of A

            bool[] lowA = a.GetLowNybble();
            a = a.SetLowNybble(xHL.GetLowNybble());
            xHL = xHL.SetLowNybble(xHL.GetHighNybble());
            xHL = xHL.SetHighNybble(lowA);

            cpu.Memory.WriteByteAt(cpu.Registers.HL, xHL);
            cpu.Registers.A = a;

            if ((sbyte)a < 0) flags.Sign = true;
            if (a == 0) flags.Zero = true;
            if (a.CountBits(true) % 2 == 0) flags.ParityOverflow = true;

            return new ExecutionResult(package, cpu.Registers.Flags, false);
        }

        public RRD()
        {
        }
    }
}

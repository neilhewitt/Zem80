using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RRD : IInstructionImplementation
    {
        public ExecutionResult Execute(IProcessor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();

            byte hl = cpu.Memory.ReadByteAt(cpu.Registers.HL);
            byte a = cpu.Registers.A;

            bool[] aBits = a.GetBits(0, 4); // store low-order bits of A
            a = a.SetBits(0, hl.GetBits(0, 4)); // low-order bits of (HL) to low-order bits of A
            hl = hl.SetBits(0, hl.GetBits(4, 4)); // high-order bits of (HL) to low-order bits of (HL)
            hl = hl.SetBits(4, aBits); // stored low-order bits of A to high-order bits of (HL)

            cpu.Memory.WriteByteAt(cpu.Registers.HL, hl);
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

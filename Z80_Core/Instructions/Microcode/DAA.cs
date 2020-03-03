using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class DAA : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;
            byte A = r.A;

            // test low 4 bits - if > 9 in BCD or HalfCarry is set, adjust +/- 0x06
            if (flags.HalfCarry || (A & 0x0F) > 0x09)
            {
                A += (byte)((flags.Subtract) ? 0xFA : 0x06);
            }

            // test high 4 bits - if > 9 in BDC or Carry is set, adjust +/- 0x60
            if (flags.Carry || (A > 0x99))
            {
                flags.Carry = true;
                A += (byte)((flags.Subtract) ? 0xA0 : 0x60);
            }

            flags.Zero = A == 0;
            flags.Sign = (A & (1 << 7)) != 0; // test bit 7
            flags.ParityOverflow = A % 2 == 0; // test even parity

            if (flags.Subtract && !flags.HalfCarry)
            {
                flags.HalfCarry = false;
            }
            else if (flags.Subtract && flags.HalfCarry)
            {
                flags.HalfCarry = ((A & 0x0F) < 0x06);
            }
            else
            {
                flags.HalfCarry = ((A & 0x0F) >= 0x0A);
            }

            r.A = A;

            return new ExecutionResult(package, flags, false);
        }

        public DAA()
        {
        }
    }
}

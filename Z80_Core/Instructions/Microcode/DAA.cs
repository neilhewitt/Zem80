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
            byte correction = 0x00;

            if (A > 0x99 || flags.Carry)
            {
                correction = 0x60;
                flags.Carry = true;
            }
            else
            {
                flags.Carry = false;
            }

            if ((A & 0x0F) > 0x09 || flags.HalfCarry)
            {
                correction += 0x06;
            }

            if (flags.Subtract)
            {
                A -= correction;
            }
            else
            {
                A += correction;
            }

            flags.Zero = (A == 0);
            flags.HalfCarry = ((byte)(r.A ^ A)).GetBit(4);

            r.A = A;

            return new ExecutionResult(package, flags, false);
        }

        public DAA()
        {
        }
    }
}

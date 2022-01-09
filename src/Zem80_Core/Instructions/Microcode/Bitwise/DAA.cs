using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class DAA : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags;
            Registers r = cpu.Registers;
            byte A = r.A;
            int mode = 0;

            if ((A & 0x0F) > 0x09 || flags.HalfCarry)
            {
                mode++;
            }

            if (A > 0x99 || flags.Carry)
            {
                mode += 2;
                flags.Carry = true;
            }

            if (flags.Subtract && !flags.HalfCarry)
            {
                flags.HalfCarry = false;
            }
            else
            {
                if (flags.Subtract && flags.HalfCarry)
                {
                    flags.HalfCarry = (A & 0x0F) < 0x06;
                }
                else
                {
                    flags.HalfCarry = (A & 0x0F) >= 0x0A;
                }
            }

            A = mode switch
            {
                1 => (byte)(A + (byte)(flags.Subtract ? 0xFA : 0x06)),
                2 => (byte)(A + (byte)(flags.Subtract ? 0xA0 : 0x60)),
                3 => (byte)(A + (byte)(flags.Subtract ? 0x9A : 0x66)),
                _ => A
            };

            flags.Sign = (A & 0x80) > 0;
            flags.Zero = (A == 0);
            flags.ParityOverflow = A.EvenParity();
            flags.X = (A & 0x08) > 0; // copy bit 3
            flags.Y = (A & 0x20) > 0; // copy bit 5

            r.A = A;

            return new ExecutionResult(package, flags);
        }

        public DAA()
        {
        }
    }
}

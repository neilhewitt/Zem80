using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RRA : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            byte value = r.A;
            bool carry = value.GetBit(0);
            value = (byte)(value >> 1);
            value = value.SetBit(7, flags.Carry);
            flags.HalfCarry = false;
            flags.Subtract = false;
            flags.Carry = carry;
            flags.X = (value & 0x08) > 0; // copy bit 3
            flags.Y = (value & 0x20) > 0; // copy bit 5

            r.A = value;

            return new ExecutionResult(package, flags);
        }

        public RRA()
        {
        }
    }
}

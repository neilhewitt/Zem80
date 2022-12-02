using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class RLA : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = cpu.Flags.Clone();
            Registers r = cpu.Registers;

            byte value = r.A;
            bool carry = value.GetBit(7);
            value = (byte)(value << 1);
            value = value.SetBit(0, flags.Carry);
            flags.Carry = carry;
            flags.HalfCarry = false;
            flags.Subtract = false;
            flags.X = (value & 0x08) > 0; // copy bit 3
            flags.Y = (value & 0x20) > 0; // copy bit 5

            r.A = value;

            return new ExecutionResult(package, flags);
        }

        public RLA()
        {
        }
    }
}

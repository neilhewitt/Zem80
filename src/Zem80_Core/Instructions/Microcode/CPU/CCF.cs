using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class CCF : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = cpu.Flags.Clone();
            bool carry = flags.Carry;
            flags.Carry = !carry;
            flags.HalfCarry = carry;
            flags.Subtract = false;
            flags.X = (cpu.Registers.A & 0x08) > 0; // copy bit 3
            flags.Y = (cpu.Registers.A & 0x20) > 0; // copy bit 5
            return new ExecutionResult(package, flags);
        }

        public CCF()
        {
        }
    }
}

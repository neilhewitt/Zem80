using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CCF : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = new Flags();
            flags.HalfCarry = cpu.Registers.Flags.HalfCarry;
            flags.Carry = !cpu.Registers.Flags.Carry;
            return new ExecutionResult(flags, 0);
        }

        public CCF()
        {
        }
    }
}

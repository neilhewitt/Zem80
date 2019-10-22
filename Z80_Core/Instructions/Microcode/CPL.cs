using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CPL : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = new Flags();
            flags.HalfCarry = true;
            flags.Subtract = true;
            cpu.Registers.A ^= cpu.Registers.A;

            return new ExecutionResult(flags, 0);
        }

        public CPL()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SCF : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Flags flags = cpu.Registers.Flags;
            flags.Carry = true;
            flags.HalfCarry = false;
            flags.Subtract = false;
            return new ExecutionResult(package, flags);
        }

        public SCF()
        {
        }
    }
}

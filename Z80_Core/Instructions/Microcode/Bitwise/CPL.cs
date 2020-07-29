using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CPL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Flags flags = cpu.Registers.Flags;
            flags.HalfCarry = true;
            flags.Subtract = true;
            cpu.Registers.A = (byte)(~cpu.Registers.A);

            return new ExecutionResult(package, flags);
        }

        public CPL()
        {
        }
    }
}

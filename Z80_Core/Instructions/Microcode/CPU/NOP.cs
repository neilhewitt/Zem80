using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class NOP : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            return new ExecutionResult(package, cpu.Registers.Flags, false, false);
        }

        public NOP()
        {
        }
    }

    public class NOP2 : NOP
    {
        public NOP2() : base()
        {
        }
    }
}
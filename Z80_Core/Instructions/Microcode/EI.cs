using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class EI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            cpu.EnableInterrupts();
            return new ExecutionResult(package, cpu.Registers.Flags, false);
        }

        public EI()
        {
        }
    }
}

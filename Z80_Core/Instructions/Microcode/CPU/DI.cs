using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class DI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            cpu.DisableInterrupts();
            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public DI()
        {
        }
    }
}

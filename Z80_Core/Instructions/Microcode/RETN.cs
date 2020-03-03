using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RETN : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            cpu.Pop(RegisterPairName.PC);
            return new ExecutionResult(package, cpu.Registers.Flags, false, true);
        }

        public RETN()
        {
        }
    }
}

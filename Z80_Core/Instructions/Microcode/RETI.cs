using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RETI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            cpu.Pop(WordRegister.PC);
            return new ExecutionResult(package, cpu.Registers.Flags, false, true);
        }

        public RETI()
        {
        }
    }
}

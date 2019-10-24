using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class DI : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.DisableInterrupts();
            return new ExecutionResult(new Flags(), 0);
        }

        public DI()
        {
        }
    }
}

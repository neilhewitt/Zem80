using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class EI : IInstructionImplementation
    {
        public ExecutionResult Execute(IProcessor cpu, InstructionPackage package)
        {
            cpu.EnableInterrupts();
            return new ExecutionResult(package, cpu.Registers.Flags, false);
        }

        public EI()
        {
        }
    }
}

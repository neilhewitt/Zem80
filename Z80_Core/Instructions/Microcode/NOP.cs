using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class NOP : IInstructionImplementation
    {
        public ExecutionResult Execute(IProcessor cpu, InstructionPackage package)
        {
            return new ExecutionResult(package, cpu.Registers.Flags, false);
        }

        public NOP()
        {
        }
    }
}

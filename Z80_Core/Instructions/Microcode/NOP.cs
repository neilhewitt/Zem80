using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class NOP : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            return new ExecutionResult(package, new Flags(), false);
        }

        public NOP()
        {
        }
    }
}

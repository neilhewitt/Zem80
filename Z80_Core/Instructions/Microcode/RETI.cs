
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RETI : IInstructionImplementation
    {
        public ExecutionResult Execute(InstructionPackage package)
        {
            return new ExecutionResult(new Flags(), 0);
        }
    }
}

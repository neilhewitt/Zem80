
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RRCA : IInstructionImplementation
    {
        public ExecutionResult Execute(InstructionPackage package)
        {
            return new ExecutionResult(new Flags(), 0);
        }
    }
}

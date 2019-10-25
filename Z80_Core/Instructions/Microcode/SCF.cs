using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SCF : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            return new ExecutionResult(new Flags() { Carry = true }, 0);
        }

        public SCF()
        {
        }
    }
}

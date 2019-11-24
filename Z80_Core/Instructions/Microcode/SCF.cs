using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SCF : IInstructionImplementation
    {
        public ExecutionResult Execute(IProcessor cpu, InstructionPackage package)
        {
            return new ExecutionResult(package, new Flags() { Carry = true }, false);
        }

        public SCF()
        {
        }
    }
}

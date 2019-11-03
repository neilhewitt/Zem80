using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class DAA : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;


            return new ExecutionResult(package, new Flags(), false);
        }

        public DAA()
        {
        }
    }
}

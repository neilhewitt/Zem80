using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class HALT : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            cpu.Halt(); // TODO: handle interrupt resume

            return new ExecutionResult(new Flags(), 0);
        }

        public HALT()
        {
        }
    }
}

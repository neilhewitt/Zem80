using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class EXX : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            cpu.Registers.ExchangeBCDEHL();

            return new ExecutionResult(new Flags(), 0);
        }

        public EXX()
        {
        }
    }
}

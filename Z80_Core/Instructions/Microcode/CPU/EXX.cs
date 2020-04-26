using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class EXX : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            cpu.Registers.ExchangeBCDEHL();

            return new ExecutionResult(package, null, false, false);
        }

        public EXX()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class EXX : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            cpu.Registers.ExchangeBCDEHL();

            return new ExecutionResult(package, null);;
        }

        public EXX()
        {
        }
    }
}

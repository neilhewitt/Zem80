using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class HALT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            cpu.Halt();

            return new ExecutionResult(package, cpu.Registers.Flags, false, false);
        }

        public HALT()
        {
        }
    }
}

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

            cpu.Halt();

            return new ExecutionResult(package, cpu.Registers.Flags, false);
        }

        public HALT()
        {
        }
    }
}

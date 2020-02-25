using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RETN : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.Pop(RegisterPairName.PC);
            return new ExecutionResult(package, cpu.Registers.Flags, false, true);
        }

        public RETN()
        {
        }
    }
}

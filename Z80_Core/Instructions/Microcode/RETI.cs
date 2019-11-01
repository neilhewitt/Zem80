using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RETI : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.Registers.PC = cpu.Stack.Pop();
            return new ExecutionResult(new Flags(), 0, true);
        }

        public RETI()
        {
        }
    }
}

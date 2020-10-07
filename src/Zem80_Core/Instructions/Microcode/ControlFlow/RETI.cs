using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class RETI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            cpu.Pop(WordRegister.PC);
            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public RETI()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RETI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            cpu.Stack.Pop(WordRegister.PC);
            cpu.Registers.WZ = cpu.Registers.PC;
            return new ExecutionResult(package, null);
        }

        public RETI()
        {
        }
    }
}

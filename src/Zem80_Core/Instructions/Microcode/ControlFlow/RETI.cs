using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
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

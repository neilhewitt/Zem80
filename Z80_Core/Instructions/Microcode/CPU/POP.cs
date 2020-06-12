using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class POP : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            
            WordRegister register = instruction.GetWordRegister();
            cpu.Pop(register);

            return new ExecutionResult(package, cpu.Registers.Flags, false, false);
        }

        public POP()
        {
        }
    }
}

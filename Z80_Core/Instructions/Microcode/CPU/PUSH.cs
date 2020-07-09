using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class PUSH : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;

            WordRegister register = instruction.Target.AsWordRegister();
            cpu.Push(register);

            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public PUSH()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class PUSH : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;

            WordRegister register = instruction.Target.AsWordRegister();
            cpu.Stack.Push(register);

            return new ExecutionResult(package, null);
        }

        public PUSH()
        {
        }
    }
}

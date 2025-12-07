using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class PUSH : MicrocodeBase
    {
        // PUSH rr

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
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

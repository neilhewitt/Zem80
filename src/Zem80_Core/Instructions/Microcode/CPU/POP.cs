using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class POP : MicrocodeBase
    {
        // POP rr

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;

            WordRegister register = instruction.Target.AsWordRegister();
            cpu.Stack.Pop(register);

            return new ExecutionResult(package, null);
        }

        public POP()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class HALT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            cpu.Halt(HaltReason.HaltInstruction);

            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public HALT()
        {
        }
    }
}

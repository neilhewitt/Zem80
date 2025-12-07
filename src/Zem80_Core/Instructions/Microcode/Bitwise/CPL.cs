using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class CPL : MicrocodeBase
    {
        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Flags flags = cpu.Flags.Clone();
            flags.HalfCarry = true;
            flags.Subtract = true;
            cpu.Registers.A = (byte)(~cpu.Registers.A);
            flags.X = (cpu.Registers.A & 0x08) > 0; // copy bit 3
            flags.Y = (cpu.Registers.A & 0x20) > 0; // copy bit 5

            return new ExecutionResult(package, flags);
        }

        public CPL()
        {
        }
    }
}

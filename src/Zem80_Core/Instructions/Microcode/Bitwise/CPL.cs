using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
{
    public class CPL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class IM : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            switch (instruction.Opcode)
            {
                case 0x46: // IM 0
                    cpu.SetInterruptMode(InterruptMode.IM0);
                    break;
                case 0x56: // IM 1
                    cpu.SetInterruptMode(InterruptMode.IM1);
                    break;
                case 0x5E: // IM 2
                    cpu.SetInterruptMode(InterruptMode.IM2);
                    break;
            }

            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public IM()
        {
        }
    }
}

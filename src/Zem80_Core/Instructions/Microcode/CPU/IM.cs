using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
{
    public class IM : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;

            switch (instruction.Opcode)
            {
                case 0x46:
                case 0x4E:
                case 0x66: // IM 0
                    cpu.SetInterruptMode(InterruptMode.IM0);
                    break;
                case 0x56:
                case 0x76: // IM 1
                    cpu.SetInterruptMode(InterruptMode.IM1);
                    break;
                case 0x5E: 
                case 0x7E: // IM 2
                    cpu.SetInterruptMode(InterruptMode.IM2);
                    break;
            }

            return new ExecutionResult(package, null);
        }

        public IM()
        {
        }
    }
}

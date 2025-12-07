using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class IM : IMicrocode
    {
        // IM 0
        // IM 1
        // IM 2

        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;

            switch (instruction.LastOpcodeByte)
            {
                case 0x46:
                case 0x4E:
                case 0x66: // IM 0
                    cpu.Interrupts.SetMode(InterruptMode.IM0);
                    break;
                case 0x56:
                case 0x76: // IM 1
                    cpu.Interrupts.SetMode(InterruptMode.IM1);
                    break;
                case 0x5E: 
                case 0x7E: // IM 2
                    cpu.Interrupts.SetMode(InterruptMode.IM2);
                    break;
            }

            return new ExecutionResult(package, null);
        }

        public IM()
        {
        }
    }
}

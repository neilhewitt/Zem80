using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class IM : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.ED:
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
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public IM()
        {
        }
    }
}

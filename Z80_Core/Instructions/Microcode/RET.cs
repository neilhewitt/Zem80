using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RET : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            bool pcWasSet = false;

            void ret()
            {
                cpu.Pop(RegisterWord.PC);
                pcWasSet = true;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC0: // RET NZ
                            if (!flags.Zero) ret();
                            break;
                        case 0xC8: // RET Z
                            if (flags.Zero) ret();
                            break;
                        case 0xC9: // RET
                            ret();
                            break;
                        case 0xD0: // RET NC
                            if (!flags.Carry) ret();
                            break;
                        case 0xD8: // RET C
                            if (flags.Carry) ret();
                            break;
                        case 0xE0: // RET PO
                            if (!flags.ParityOverflow) ret();
                            break;
                        case 0xE8: // RET PE
                            if (flags.ParityOverflow) ret();
                            break;
                        case 0xF0: // RET P
                            if (!flags.Sign) ret();
                            break;
                        case 0xF8: // RET M
                            if (flags.Sign) ret();
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, cpu.Registers.Flags, !pcWasSet, pcWasSet);
        }

        public RET()
        {
        }
    }
}

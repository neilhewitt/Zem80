using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RET : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IFlags flags = cpu.Registers.Flags;
            bool pcWasSet = false;

            void ret()
            {
                cpu.Pop(RegisterPair.PC);
                pcWasSet = true;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC0: // RET NZ,nn
                            if (!flags.Zero) ret();
                            break;
                        case 0xC8: // RET Z,nn
                            if (flags.Zero) ret();
                            break;
                        case 0xC9: // RET nn
                            ret();
                            break;
                        case 0xD0: // RET NC,nn
                            if (!flags.Carry) ret();
                            break;
                        case 0xD8: // RET C,nn
                            if (flags.Carry) ret();
                            break;
                        case 0xE0: // RET PO,nn
                            if (!flags.ParityOverflow) ret();
                            break;
                        case 0xE8: // RET PE,nn
                            if (flags.ParityOverflow) ret();
                            break;
                        case 0xF0: // RET P,nn
                            if (!flags.Sign) ret();
                            break;
                        case 0xF8: // RET M,nn
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

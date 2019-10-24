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
                cpu.Registers.PC = cpu.Stack.Pop();
                pcWasSet = true;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC4: // CALL NZ,nn
                            if (!flags.Zero) ret();
                            break;
                        case 0xCC: // CALL Z,nn
                            if (flags.Zero) ret();
                            break;
                        case 0xCD: // CALL nn
                            ret();
                            break;
                        case 0xD4: // CALL NC,nn
                            if (!flags.Carry) ret();
                            break;
                        case 0xDC: // CALL C,nn
                            if (flags.Carry) ret();
                            break;
                        case 0xE4: // CALL PO,nn
                            if (!flags.ParityOverflow) ret();
                            break;
                        case 0xEC: // CALL PE,nn
                            if (flags.ParityOverflow) ret();
                            break;
                        case 0xF4: // CALL P,nn
                            if (!flags.Sign) ret();
                            break;
                        case 0xFC: // CALL M,nn
                            if (flags.Sign) ret();
                            break;
                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0, pcWasSet);
        }

        public RET()
        {
        }
    }
}

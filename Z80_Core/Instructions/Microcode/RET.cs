using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RET : IInstructionImplementation
    {
        public ExecutionResult Execute(InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC0: // RET NZ
                            // code
                            break;
                        case 0xC8: // RET Z
                            // code
                            break;
                        case 0xC9: // RET
                            // code
                            break;
                        case 0xD0: // RET NC
                            // code
                            break;
                        case 0xD8: // RET C
                            // code
                            break;
                        case 0xE0: // RET PO
                            // code
                            break;
                        case 0xE8: // RET PE
                            // code
                            break;
                        case 0xF0: // RET P
                            // code
                            break;
                        case 0xF8: // RET M
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0xC0: // RET NZ
                            // code
                            break;
                        case 0xC8: // RET Z
                            // code
                            break;
                        case 0xC9: // RET
                            // code
                            break;
                        case 0xD0: // RET NC
                            // code
                            break;
                        case 0xD8: // RET C
                            // code
                            break;
                        case 0xE0: // RET PO
                            // code
                            break;
                        case 0xE8: // RET PE
                            // code
                            break;
                        case 0xF0: // RET P
                            // code
                            break;
                        case 0xF8: // RET M
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x45: // RETN
                            // code
                            break;
                        case 0x4D: // RETI
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {

                    }
                    break;

                case InstructionPrefix.DDCB:
                    switch (instruction.Opcode)
                    {

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }
    }
}

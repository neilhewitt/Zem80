using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class IN : IInstructionImplementation
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
                        case 0x03: // INC BC
                            // code
                            break;
                        case 0x04: // INC B
                            // code
                            break;
                        case 0x0C: // INC C
                            // code
                            break;
                        case 0x13: // INC DE
                            // code
                            break;
                        case 0x14: // INC D
                            // code
                            break;
                        case 0x1C: // INC E
                            // code
                            break;
                        case 0x23: // INC HL
                            // code
                            break;
                        case 0x24: // INC H
                            // code
                            break;
                        case 0x2C: // INC L
                            // code
                            break;
                        case 0x33: // INC SP
                            // code
                            break;
                        case 0x34: // INC (HL)
                            // code
                            break;
                        case 0x3C: // INC A
                            // code
                            break;
                        case 0xDB: // IN A,(n)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x03: // INC BC
                            // code
                            break;
                        case 0x04: // INC B
                            // code
                            break;
                        case 0x0C: // INC C
                            // code
                            break;
                        case 0x13: // INC DE
                            // code
                            break;
                        case 0x14: // INC D
                            // code
                            break;
                        case 0x1C: // INC E
                            // code
                            break;
                        case 0x23: // INC HL
                            // code
                            break;
                        case 0x24: // INC H
                            // code
                            break;
                        case 0x2C: // INC L
                            // code
                            break;
                        case 0x33: // INC SP
                            // code
                            break;
                        case 0x34: // INC (HL)
                            // code
                            break;
                        case 0x3C: // INC A
                            // code
                            break;
                        case 0xDB: // IN A,(n)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x40: // IN B,(C)
                            // code
                            break;
                        case 0x48: // IN C,(C)
                            // code
                            break;
                        case 0x50: // IN D,(C)
                            // code
                            break;
                        case 0x58: // IN E,(C)
                            // code
                            break;
                        case 0x60: // IN H,(C)
                            // code
                            break;
                        case 0x68: // IN L,(C)
                            // code
                            break;
                        case 0x70: // IN F,(C)
                            // code
                            break;
                        case 0x78: // IN A,(C)
                            // code
                            break;
                        case 0xA2: // INI
                            // code
                            break;
                        case 0xAA: // IND
                            // code
                            break;
                        case 0xB2: // INIR
                            // code
                            break;
                        case 0xBA: // INDR
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x24: // INC IXh
                            // code
                            break;
                        case 0x2C: // INC IXl
                            // code
                            break;
                        case 0x23: // INC IX
                            // code
                            break;
                        case 0x34: // INC (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x24: // INC IYh
                            // code
                            break;
                        case 0x2C: // INC IYl
                            // code
                            break;
                        case 0x23: // INC IY
                            // code
                            break;
                        case 0x34: // INC (IY+o)
                            // code
                            break;

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

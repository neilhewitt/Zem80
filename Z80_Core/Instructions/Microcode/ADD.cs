using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ADD : IInstructionImplementation
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
                        case 0x09: // ADD HL,BC
                            // code
                            break;
                        case 0x19: // ADD HL,DE
                            // code
                            break;
                        case 0x29: // ADD HL,HL
                            // code
                            break;
                        case 0x39: // ADD HL,SP
                            // code
                            break;
                        case 0x80: // ADD A,B
                            // code
                            break;
                        case 0x81: // ADD A,C
                            // code
                            break;
                        case 0x82: // ADD A,D
                            // code
                            break;
                        case 0x83: // ADD A,E
                            // code
                            break;
                        case 0x84: // ADD A,H
                            // code
                            break;
                        case 0x85: // ADD A,L
                            // code
                            break;
                        case 0x87: // ADD A,A
                            // code
                            break;
                        case 0x86: // ADD A,(HL)
                            // code
                            break;
                        case 0xC6: // ADD A,n
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x09: // ADD HL,BC
                            // code
                            break;
                        case 0x19: // ADD HL,DE
                            // code
                            break;
                        case 0x29: // ADD HL,HL
                            // code
                            break;
                        case 0x39: // ADD HL,SP
                            // code
                            break;
                        case 0x80: // ADD A,B
                            // code
                            break;
                        case 0x81: // ADD A,C
                            // code
                            break;
                        case 0x82: // ADD A,D
                            // code
                            break;
                        case 0x83: // ADD A,E
                            // code
                            break;
                        case 0x84: // ADD A,H
                            // code
                            break;
                        case 0x85: // ADD A,L
                            // code
                            break;
                        case 0x87: // ADD A,A
                            // code
                            break;
                        case 0x86: // ADD A,(HL)
                            // code
                            break;
                        case 0xC6: // ADD A,n
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x09: // ADD IX,BC
                            // code
                            break;
                        case 0x19: // ADD IX,DE
                            // code
                            break;
                        case 0x29: // ADD IX,IX
                            // code
                            break;
                        case 0x39: // ADD IX,SP
                            // code
                            break;
                        case 0x84: // ADD A,IXh
                            // code
                            break;
                        case 0x85: // ADD A,IXl
                            // code
                            break;
                        case 0x86: // ADD A,(IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x09: // ADD IY,BC
                            // code
                            break;
                        case 0x19: // ADD IY,DE
                            // code
                            break;
                        case 0x29: // ADD IY,IY
                            // code
                            break;
                        case 0x39: // ADD IY,SP
                            // code
                            break;
                        case 0x84: // ADD A,IYh
                            // code
                            break;
                        case 0x85: // ADD A,IYl
                            // code
                            break;
                        case 0x86: // ADD A,(IY+o)
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

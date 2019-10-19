using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class BIT : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x40: // BIT 0,B
                            // code
                            break;
                        case 0x48: // BIT 1,B
                            // code
                            break;
                        case 0x50: // BIT 2,B
                            // code
                            break;
                        case 0x58: // BIT 3,B
                            // code
                            break;
                        case 0x60: // BIT 4,B
                            // code
                            break;
                        case 0x68: // BIT 5,B
                            // code
                            break;
                        case 0x70: // BIT 6,B
                            // code
                            break;
                        case 0x78: // BIT 7,B
                            // code
                            break;
                        case 0x41: // BIT 0,C
                            // code
                            break;
                        case 0x49: // BIT 1,C
                            // code
                            break;
                        case 0x51: // BIT 2,C
                            // code
                            break;
                        case 0x59: // BIT 3,C
                            // code
                            break;
                        case 0x61: // BIT 4,C
                            // code
                            break;
                        case 0x69: // BIT 5,C
                            // code
                            break;
                        case 0x71: // BIT 6,C
                            // code
                            break;
                        case 0x79: // BIT 7,C
                            // code
                            break;
                        case 0x42: // BIT 0,D
                            // code
                            break;
                        case 0x4A: // BIT 1,D
                            // code
                            break;
                        case 0x52: // BIT 2,D
                            // code
                            break;
                        case 0x5A: // BIT 3,D
                            // code
                            break;
                        case 0x62: // BIT 4,D
                            // code
                            break;
                        case 0x6A: // BIT 5,D
                            // code
                            break;
                        case 0x72: // BIT 6,D
                            // code
                            break;
                        case 0x7A: // BIT 7,D
                            // code
                            break;
                        case 0x43: // BIT 0,E
                            // code
                            break;
                        case 0x4B: // BIT 1,E
                            // code
                            break;
                        case 0x53: // BIT 2,E
                            // code
                            break;
                        case 0x5B: // BIT 3,E
                            // code
                            break;
                        case 0x63: // BIT 4,E
                            // code
                            break;
                        case 0x6B: // BIT 5,E
                            // code
                            break;
                        case 0x73: // BIT 6,E
                            // code
                            break;
                        case 0x7B: // BIT 7,E
                            // code
                            break;
                        case 0x44: // BIT 0,H
                            // code
                            break;
                        case 0x4C: // BIT 1,H
                            // code
                            break;
                        case 0x54: // BIT 2,H
                            // code
                            break;
                        case 0x5C: // BIT 3,H
                            // code
                            break;
                        case 0x64: // BIT 4,H
                            // code
                            break;
                        case 0x6C: // BIT 5,H
                            // code
                            break;
                        case 0x74: // BIT 6,H
                            // code
                            break;
                        case 0x7C: // BIT 7,H
                            // code
                            break;
                        case 0x45: // BIT 0,L
                            // code
                            break;
                        case 0x4D: // BIT 1,L
                            // code
                            break;
                        case 0x55: // BIT 2,L
                            // code
                            break;
                        case 0x5D: // BIT 3,L
                            // code
                            break;
                        case 0x65: // BIT 4,L
                            // code
                            break;
                        case 0x6D: // BIT 5,L
                            // code
                            break;
                        case 0x75: // BIT 6,L
                            // code
                            break;
                        case 0x7D: // BIT 7,L
                            // code
                            break;
                        case 0x46: // BIT 0,(HL)
                            // code
                            break;
                        case 0x4E: // BIT 1,(HL)
                            // code
                            break;
                        case 0x56: // BIT 2,(HL)
                            // code
                            break;
                        case 0x5E: // BIT 3,(HL)
                            // code
                            break;
                        case 0x66: // BIT 4,(HL)
                            // code
                            break;
                        case 0x6E: // BIT 5,(HL)
                            // code
                            break;
                        case 0x76: // BIT 6,(HL)
                            // code
                            break;
                        case 0x7E: // BIT 7,(HL)
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
                        case 0x46: // BIT 0,(IX+o)
                            // code
                            break;
                        case 0x4E: // BIT 1,(IX+o)
                            // code
                            break;
                        case 0x56: // BIT 2,(IX+o)
                            // code
                            break;
                        case 0x5E: // BIT 3,(IX+o)
                            // code
                            break;
                        case 0x66: // BIT 4,(IX+o)
                            // code
                            break;
                        case 0x6E: // BIT 5,(IX+o)
                            // code
                            break;
                        case 0x76: // BIT 6,(IX+o)
                            // code
                            break;
                        case 0x7E: // BIT 7,(IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0x46: // BIT 0,(IY+o)
                            // code
                            break;
                        case 0x4E: // BIT 1,(IY+o)
                            // code
                            break;
                        case 0x56: // BIT 2,(IY+o)
                            // code
                            break;
                        case 0x5E: // BIT 3,(IY+o)
                            // code
                            break;
                        case 0x66: // BIT 4,(IY+o)
                            // code
                            break;
                        case 0x6E: // BIT 5,(IY+o)
                            // code
                            break;
                        case 0x76: // BIT 6,(IY+o)
                            // code
                            break;
                        case 0x7E: // BIT 7,(IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public BIT()
        {
        }
    }
}

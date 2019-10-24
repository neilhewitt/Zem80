using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RES : IInstructionImplementation
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
                        case 0x80: // RES 0,B
                            // code
                            break;
                        case 0x88: // RES 1,B
                            // code
                            break;
                        case 0x90: // RES 2,B
                            // code
                            break;
                        case 0x98: // RES 3,B
                            // code
                            break;
                        case 0xA0: // RES 4,B
                            // code
                            break;
                        case 0xA8: // RES 5,B
                            // code
                            break;
                        case 0xB0: // RES 6,B
                            // code
                            break;
                        case 0xB8: // RES 7,B
                            // code
                            break;
                        case 0x81: // RES 0,C
                            // code
                            break;
                        case 0x89: // RES 1,C
                            // code
                            break;
                        case 0x91: // RES 2,C
                            // code
                            break;
                        case 0x99: // RES 3,C
                            // code
                            break;
                        case 0xA1: // RES 4,C
                            // code
                            break;
                        case 0xA9: // RES 5,C
                            // code
                            break;
                        case 0xB1: // RES 6,C
                            // code
                            break;
                        case 0xB9: // RES 7,C
                            // code
                            break;
                        case 0x82: // RES 0,D
                            // code
                            break;
                        case 0x8A: // RES 1,D
                            // code
                            break;
                        case 0x92: // RES 2,D
                            // code
                            break;
                        case 0x9A: // RES 3,D
                            // code
                            break;
                        case 0xA2: // RES 4,D
                            // code
                            break;
                        case 0xAA: // RES 5,D
                            // code
                            break;
                        case 0xB2: // RES 6,D
                            // code
                            break;
                        case 0xBA: // RES 7,D
                            // code
                            break;
                        case 0x83: // RES 0,E
                            // code
                            break;
                        case 0x8B: // RES 1,E
                            // code
                            break;
                        case 0x93: // RES 2,E
                            // code
                            break;
                        case 0x9B: // RES 3,E
                            // code
                            break;
                        case 0xA3: // RES 4,E
                            // code
                            break;
                        case 0xAB: // RES 5,E
                            // code
                            break;
                        case 0xB3: // RES 6,E
                            // code
                            break;
                        case 0xBB: // RES 7,E
                            // code
                            break;
                        case 0x84: // RES 0,H
                            // code
                            break;
                        case 0x8C: // RES 1,H
                            // code
                            break;
                        case 0x94: // RES 2,H
                            // code
                            break;
                        case 0x9C: // RES 3,H
                            // code
                            break;
                        case 0xA4: // RES 4,H
                            // code
                            break;
                        case 0xAC: // RES 5,H
                            // code
                            break;
                        case 0xB4: // RES 6,H
                            // code
                            break;
                        case 0xBC: // RES 7,H
                            // code
                            break;
                        case 0x85: // RES 0,L
                            // code
                            break;
                        case 0x8D: // RES 1,L
                            // code
                            break;
                        case 0x95: // RES 2,L
                            // code
                            break;
                        case 0x9D: // RES 3,L
                            // code
                            break;
                        case 0xA5: // RES 4,L
                            // code
                            break;
                        case 0xAD: // RES 5,L
                            // code
                            break;
                        case 0xB5: // RES 6,L
                            // code
                            break;
                        case 0xBD: // RES 7,L
                            // code
                            break;
                        case 0x86: // RES 0,(HL)
                            // code
                            break;
                        case 0x8E: // RES 1,(HL)
                            // code
                            break;
                        case 0x96: // RES 2,(HL)
                            // code
                            break;
                        case 0x9E: // RES 3,(HL)
                            // code
                            break;
                        case 0xA6: // RES 4,(HL)
                            // code
                            break;
                        case 0xAE: // RES 5,(HL)
                            // code
                            break;
                        case 0xB6: // RES 6,(HL)
                            // code
                            break;
                        case 0xBE: // RES 7,(HL)
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
                        case 0x86: // RES 0,(IX+o)
                            // code
                            break;
                        case 0x8E: // RES 1,(IX+o)
                            // code
                            break;
                        case 0x96: // RES 2,(IX+o)
                            // code
                            break;
                        case 0x9E: // RES 3,(IX+o)
                            // code
                            break;
                        case 0xA6: // RES 4,(IX+o)
                            // code
                            break;
                        case 0xAE: // RES 5,(IX+o)
                            // code
                            break;
                        case 0xB6: // RES 6,(IX+o)
                            // code
                            break;
                        case 0xBE: // RES 7,(IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0x86: // RES 0,(IY+o)
                            // code
                            break;
                        case 0x8E: // RES 1,(IY+o)
                            // code
                            break;
                        case 0x96: // RES 2,(IY+o)
                            // code
                            break;
                        case 0x9E: // RES 3,(IY+o)
                            // code
                            break;
                        case 0xA6: // RES 4,(IY+o)
                            // code
                            break;
                        case 0xAE: // RES 5,(IY+o)
                            // code
                            break;
                        case 0xB6: // RES 6,(IY+o)
                            // code
                            break;
                        case 0xBE: // RES 7,(IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public RES()
        {
        }
    }
}

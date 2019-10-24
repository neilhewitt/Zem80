using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SET : IInstructionImplementation
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
                        case 0xC0: // SET 0,B
                            // code
                            break;
                        case 0xC8: // SET 1,B
                            // code
                            break;
                        case 0xD0: // SET 2,B
                            // code
                            break;
                        case 0xD8: // SET 3,B
                            // code
                            break;
                        case 0xE0: // SET 4,B
                            // code
                            break;
                        case 0xE8: // SET 5,B
                            // code
                            break;
                        case 0xF0: // SET 6,B
                            // code
                            break;
                        case 0xF8: // SET 7,B
                            // code
                            break;
                        case 0xC1: // SET 0,C
                            // code
                            break;
                        case 0xC9: // SET 1,C
                            // code
                            break;
                        case 0xD1: // SET 2,C
                            // code
                            break;
                        case 0xD9: // SET 3,C
                            // code
                            break;
                        case 0xE1: // SET 4,C
                            // code
                            break;
                        case 0xE9: // SET 5,C
                            // code
                            break;
                        case 0xF1: // SET 6,C
                            // code
                            break;
                        case 0xF9: // SET 7,C
                            // code
                            break;
                        case 0xC2: // SET 0,D
                            // code
                            break;
                        case 0xCA: // SET 1,D
                            // code
                            break;
                        case 0xD2: // SET 2,D
                            // code
                            break;
                        case 0xDA: // SET 3,D
                            // code
                            break;
                        case 0xE2: // SET 4,D
                            // code
                            break;
                        case 0xEA: // SET 5,D
                            // code
                            break;
                        case 0xF2: // SET 6,D
                            // code
                            break;
                        case 0xFA: // SET 7,D
                            // code
                            break;
                        case 0xC3: // SET 0,E
                            // code
                            break;
                        case 0xCB: // SET 1,E
                            // code
                            break;
                        case 0xD3: // SET 2,E
                            // code
                            break;
                        case 0xDB: // SET 3,E
                            // code
                            break;
                        case 0xE3: // SET 4,E
                            // code
                            break;
                        case 0xEB: // SET 5,E
                            // code
                            break;
                        case 0xF3: // SET 6,E
                            // code
                            break;
                        case 0xFB: // SET 7,E
                            // code
                            break;
                        case 0xC4: // SET 0,H
                            // code
                            break;
                        case 0xCC: // SET 1,H
                            // code
                            break;
                        case 0xD4: // SET 2,H
                            // code
                            break;
                        case 0xDC: // SET 3,H
                            // code
                            break;
                        case 0xE4: // SET 4,H
                            // code
                            break;
                        case 0xEC: // SET 5,H
                            // code
                            break;
                        case 0xF4: // SET 6,H
                            // code
                            break;
                        case 0xFC: // SET 7,H
                            // code
                            break;
                        case 0xC5: // SET 0,L
                            // code
                            break;
                        case 0xCD: // SET 1,L
                            // code
                            break;
                        case 0xD5: // SET 2,L
                            // code
                            break;
                        case 0xDD: // SET 3,L
                            // code
                            break;
                        case 0xE5: // SET 4,L
                            // code
                            break;
                        case 0xED: // SET 5,L
                            // code
                            break;
                        case 0xF5: // SET 6,L
                            // code
                            break;
                        case 0xFD: // SET 7,L
                            // code
                            break;
                        case 0xC6: // SET 0,(HL)
                            // code
                            break;
                        case 0xCE: // SET 1,(HL)
                            // code
                            break;
                        case 0xD6: // SET 2,(HL)
                            // code
                            break;
                        case 0xDE: // SET 3,(HL)
                            // code
                            break;
                        case 0xE6: // SET 4,(HL)
                            // code
                            break;
                        case 0xEE: // SET 5,(HL)
                            // code
                            break;
                        case 0xF6: // SET 6,(HL)
                            // code
                            break;
                        case 0xFE: // SET 7,(HL)
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
                        case 0xC6: // SET 0,(IX+o)
                            // code
                            break;
                        case 0xCE: // SET 1,(IX+o)
                            // code
                            break;
                        case 0xD6: // SET 2,(IX+o)
                            // code
                            break;
                        case 0xDE: // SET 3,(IX+o)
                            // code
                            break;
                        case 0xE6: // SET 4,(IX+o)
                            // code
                            break;
                        case 0xEE: // SET 5,(IX+o)
                            // code
                            break;
                        case 0xF6: // SET 6,(IX+o)
                            // code
                            break;
                        case 0xFE: // SET 7,(IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0xC6: // SET 0,(IY+o)
                            // code
                            break;
                        case 0xCE: // SET 1,(IY+o)
                            // code
                            break;
                        case 0xD6: // SET 2,(IY+o)
                            // code
                            break;
                        case 0xDE: // SET 3,(IY+o)
                            // code
                            break;
                        case 0xE6: // SET 4,(IY+o)
                            // code
                            break;
                        case 0xEE: // SET 5,(IY+o)
                            // code
                            break;
                        case 0xF6: // SET 6,(IY+o)
                            // code
                            break;
                        case 0xFE: // SET 7,(IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public SET()
        {
        }
    }
}

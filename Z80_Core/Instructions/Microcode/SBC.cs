using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SBC : IInstructionImplementation
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
                        case 0x98: // SBC A,B
                            // code
                            break;
                        case 0x99: // SBC A,C
                            // code
                            break;
                        case 0x9A: // SBC A,D
                            // code
                            break;
                        case 0x9B: // SBC A,E
                            // code
                            break;
                        case 0x9C: // SBC A,H
                            // code
                            break;
                        case 0x9D: // SBC A,L
                            // code
                            break;
                        case 0x9F: // SBC A,A
                            // code
                            break;
                        case 0x9E: // SBC A,(HL)
                            // code
                            break;
                        case 0xDE: // SBC A,n
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {

                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x42: // SBC HL,BC
                            // code
                            break;
                        case 0x52: // SBC HL,DE
                            // code
                            break;
                        case 0x62: // SBC HL,HL
                            // code
                            break;
                        case 0x72: // SBC HL,SP
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x9C: // SBC A,IXh
                            // code
                            break;
                        case 0x9D: // SBC A,IXl
                            // code
                            break;
                        case 0x9E: // SBC A,(IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x9C: // SBC A,IYh
                            // code
                            break;
                        case 0x9D: // SBC A,IYl
                            // code
                            break;
                        case 0x9E: // SBC A,(IY+o)
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

        public SBC()
        {
        }
    }
}

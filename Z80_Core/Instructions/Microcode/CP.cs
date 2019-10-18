using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CP : IInstructionImplementation
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
                        case 0x2F: // CPL
                            // code
                            break;
                        case 0xB8: // CP B
                            // code
                            break;
                        case 0xB9: // CP C
                            // code
                            break;
                        case 0xBA: // CP D
                            // code
                            break;
                        case 0xBB: // CP E
                            // code
                            break;
                        case 0xBC: // CP H
                            // code
                            break;
                        case 0xBD: // CP L
                            // code
                            break;
                        case 0xBF: // CP A
                            // code
                            break;
                        case 0xBE: // CP (HL)
                            // code
                            break;
                        case 0xFE: // CP n
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x2F: // CPL
                            // code
                            break;
                        case 0xB8: // CP B
                            // code
                            break;
                        case 0xB9: // CP C
                            // code
                            break;
                        case 0xBA: // CP D
                            // code
                            break;
                        case 0xBB: // CP E
                            // code
                            break;
                        case 0xBC: // CP H
                            // code
                            break;
                        case 0xBD: // CP L
                            // code
                            break;
                        case 0xBF: // CP A
                            // code
                            break;
                        case 0xBE: // CP (HL)
                            // code
                            break;
                        case 0xFE: // CP n
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0xA1: // CPI
                            // code
                            break;
                        case 0xA9: // CPD
                            // code
                            break;
                        case 0xB1: // CPIR
                            // code
                            break;
                        case 0xB9: // CPDR
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xBC: // CP IXh
                            // code
                            break;
                        case 0xBD: // CP IXl
                            // code
                            break;
                        case 0xBE: // CP (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xBC: // CP IYh
                            // code
                            break;
                        case 0xBD: // CP IYl
                            // code
                            break;
                        case 0xBE: // CP (IY+o)
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

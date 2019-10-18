using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ADC : IInstructionImplementation
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
                        case 0x88: // ADC A,B
                            // code
                            break;
                        case 0x89: // ADC A,C
                            // code
                            break;
                        case 0x8A: // ADC A,D
                            // code
                            break;
                        case 0x8B: // ADC A,E
                            // code
                            break;
                        case 0x8C: // ADC A,H
                            // code
                            break;
                        case 0x8D: // ADC A,L
                            // code
                            break;
                        case 0x8F: // ADC A,A
                            // code
                            break;
                        case 0x8E: // ADC A,(HL)
                            // code
                            break;
                        case 0xCE: // ADC A,n
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x88: // ADC A,B
                            // code
                            break;
                        case 0x89: // ADC A,C
                            // code
                            break;
                        case 0x8A: // ADC A,D
                            // code
                            break;
                        case 0x8B: // ADC A,E
                            // code
                            break;
                        case 0x8C: // ADC A,H
                            // code
                            break;
                        case 0x8D: // ADC A,L
                            // code
                            break;
                        case 0x8F: // ADC A,A
                            // code
                            break;
                        case 0x8E: // ADC A,(HL)
                            // code
                            break;
                        case 0xCE: // ADC A,n
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x4A: // ADC HL,BC
                            // code
                            break;
                        case 0x5A: // ADC HL,DE
                            // code
                            break;
                        case 0x6A: // ADC HL,HL
                            // code
                            break;
                        case 0x7A: // ADC HL,SP
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x8C: // ADC A,IXh
                            // code
                            break;
                        case 0x8D: // ADC A,IXl
                            // code
                            break;
                        case 0x8E: // ADC A,(IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x8C: // ADC A,IYh
                            // code
                            break;
                        case 0x8D: // ADC A,IYl
                            // code
                            break;
                        case 0x8E: // ADC A,(IY+o)
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

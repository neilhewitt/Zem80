using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class OR : IInstructionImplementation
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
                        case 0xB0: // OR B
                            // code
                            break;
                        case 0xB1: // OR C
                            // code
                            break;
                        case 0xB2: // OR D
                            // code
                            break;
                        case 0xB3: // OR E
                            // code
                            break;
                        case 0xB4: // OR H
                            // code
                            break;
                        case 0xB5: // OR L
                            // code
                            break;
                        case 0xB7: // OR A
                            // code
                            break;
                        case 0xB6: // OR (HL)
                            // code
                            break;
                        case 0xF6: // OR n
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0xB0: // OR B
                            // code
                            break;
                        case 0xB1: // OR C
                            // code
                            break;
                        case 0xB2: // OR D
                            // code
                            break;
                        case 0xB3: // OR E
                            // code
                            break;
                        case 0xB4: // OR H
                            // code
                            break;
                        case 0xB5: // OR L
                            // code
                            break;
                        case 0xB7: // OR A
                            // code
                            break;
                        case 0xB6: // OR (HL)
                            // code
                            break;
                        case 0xF6: // OR n
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
                        case 0xB4: // OR IXh
                            // code
                            break;
                        case 0xB5: // OR IXl
                            // code
                            break;
                        case 0xB6: // OR (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xB4: // OR IYh
                            // code
                            break;
                        case 0xB5: // OR IYl
                            // code
                            break;
                        case 0xB6: // OR (IY+o)
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

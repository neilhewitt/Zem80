using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class AND : IInstructionImplementation
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
                        case 0xA0: // AND B
                            // code
                            break;
                        case 0xA1: // AND C
                            // code
                            break;
                        case 0xA2: // AND D
                            // code
                            break;
                        case 0xA3: // AND E
                            // code
                            break;
                        case 0xA4: // AND H
                            // code
                            break;
                        case 0xA5: // AND L
                            // code
                            break;
                        case 0xA7: // AND A
                            // code
                            break;
                        case 0xA6: // AND (HL)
                            // code
                            break;
                        case 0xE6: // AND n
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0xA0: // AND B
                            // code
                            break;
                        case 0xA1: // AND C
                            // code
                            break;
                        case 0xA2: // AND D
                            // code
                            break;
                        case 0xA3: // AND E
                            // code
                            break;
                        case 0xA4: // AND H
                            // code
                            break;
                        case 0xA5: // AND L
                            // code
                            break;
                        case 0xA7: // AND A
                            // code
                            break;
                        case 0xA6: // AND (HL)
                            // code
                            break;
                        case 0xE6: // AND n
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
                        case 0xA4: // AND IXh
                            // code
                            break;
                        case 0xA5: // AND IXl
                            // code
                            break;
                        case 0xA6: // AND (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xA4: // AND IYh
                            // code
                            break;
                        case 0xA5: // AND IYl
                            // code
                            break;
                        case 0xA6: // AND (IY+o)
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

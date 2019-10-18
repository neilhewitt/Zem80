using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class DEC : IInstructionImplementation
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
                        case 0x05: // DEC B
                            // code
                            break;
                        case 0x0B: // DEC BC
                            // code
                            break;
                        case 0x0D: // DEC C
                            // code
                            break;
                        case 0x15: // DEC D
                            // code
                            break;
                        case 0x1B: // DEC DE
                            // code
                            break;
                        case 0x1D: // DEC E
                            // code
                            break;
                        case 0x25: // DEC H
                            // code
                            break;
                        case 0x2B: // DEC HL
                            // code
                            break;
                        case 0x2D: // DEC L
                            // code
                            break;
                        case 0x35: // DEC (HL)
                            // code
                            break;
                        case 0x3B: // DEC SP
                            // code
                            break;
                        case 0x3D: // DEC A
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x05: // DEC B
                            // code
                            break;
                        case 0x0B: // DEC BC
                            // code
                            break;
                        case 0x0D: // DEC C
                            // code
                            break;
                        case 0x15: // DEC D
                            // code
                            break;
                        case 0x1B: // DEC DE
                            // code
                            break;
                        case 0x1D: // DEC E
                            // code
                            break;
                        case 0x25: // DEC H
                            // code
                            break;
                        case 0x2B: // DEC HL
                            // code
                            break;
                        case 0x2D: // DEC L
                            // code
                            break;
                        case 0x35: // DEC (HL)
                            // code
                            break;
                        case 0x3B: // DEC SP
                            // code
                            break;
                        case 0x3D: // DEC A
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
                        case 0x25: // DEC IXh
                            // code
                            break;
                        case 0x2D: // DEC IXl
                            // code
                            break;
                        case 0x2B: // DEC IX
                            // code
                            break;
                        case 0x35: // DEC (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x25: // DEC IYh
                            // code
                            break;
                        case 0x2D: // DEC IYl
                            // code
                            break;
                        case 0x2B: // DEC IY
                            // code
                            break;
                        case 0x35: // DEC (IY+o)
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

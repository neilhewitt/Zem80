using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class INC : IInstructionImplementation
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
                        case 0x03: // INC BC
                            // code
                            break;
                        case 0x04: // INC B
                            // code
                            break;
                        case 0x0C: // INC C
                            // code
                            break;
                        case 0x13: // INC DE
                            // code
                            break;
                        case 0x14: // INC D
                            // code
                            break;
                        case 0x1C: // INC E
                            // code
                            break;
                        case 0x23: // INC HL
                            // code
                            break;
                        case 0x24: // INC H
                            // code
                            break;
                        case 0x2C: // INC L
                            // code
                            break;
                        case 0x33: // INC SP
                            // code
                            break;
                        case 0x34: // INC (HL)
                            // code
                            break;
                        case 0x3C: // INC A
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x03: // INC BC
                            // code
                            break;
                        case 0x04: // INC B
                            // code
                            break;
                        case 0x0C: // INC C
                            // code
                            break;
                        case 0x13: // INC DE
                            // code
                            break;
                        case 0x14: // INC D
                            // code
                            break;
                        case 0x1C: // INC E
                            // code
                            break;
                        case 0x23: // INC HL
                            // code
                            break;
                        case 0x24: // INC H
                            // code
                            break;
                        case 0x2C: // INC L
                            // code
                            break;
                        case 0x33: // INC SP
                            // code
                            break;
                        case 0x34: // INC (HL)
                            // code
                            break;
                        case 0x3C: // INC A
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
                        case 0x24: // INC IXh
                            // code
                            break;
                        case 0x2C: // INC IXl
                            // code
                            break;
                        case 0x23: // INC IX
                            // code
                            break;
                        case 0x34: // INC (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x24: // INC IYh
                            // code
                            break;
                        case 0x2C: // INC IYl
                            // code
                            break;
                        case 0x23: // INC IY
                            // code
                            break;
                        case 0x34: // INC (IY+o)
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RL : IInstructionImplementation
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
                        case 0x07: // RLCA
                            // code
                            break;
                        case 0x17: // RLA
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x00: // RLC B
                            // code
                            break;
                        case 0x01: // RLC C
                            // code
                            break;
                        case 0x02: // RLC D
                            // code
                            break;
                        case 0x03: // RLC E
                            // code
                            break;
                        case 0x04: // RLC H
                            // code
                            break;
                        case 0x05: // RLC L
                            // code
                            break;
                        case 0x07: // RLC A
                            // code
                            break;
                        case 0x06: // RLC (HL)
                            // code
                            break;
                        case 0x10: // RL B
                            // code
                            break;
                        case 0x11: // RL C
                            // code
                            break;
                        case 0x12: // RL D
                            // code
                            break;
                        case 0x13: // RL E
                            // code
                            break;
                        case 0x14: // RL H
                            // code
                            break;
                        case 0x15: // RL L
                            // code
                            break;
                        case 0x17: // RL A
                            // code
                            break;
                        case 0x16: // RL (HL)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x6F: // RLD
                            // code
                            break;

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
                        case 0x06: // RLC (IX+o)
                            // code
                            break;
                        case 0x16: // RL (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0x06: // RLC (IY+o)
                            // code
                            break;
                        case 0x16: // RL (IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public RL()
        {
        }
    }
}

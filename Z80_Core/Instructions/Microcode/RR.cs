using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RR : IInstructionImplementation
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
                        case 0x0F: // RRCA
                            // code
                            break;
                        case 0x1F: // RRA
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x08: // RRC B
                            // code
                            break;
                        case 0x09: // RRC C
                            // code
                            break;
                        case 0x0A: // RRC D
                            // code
                            break;
                        case 0x0B: // RRC E
                            // code
                            break;
                        case 0x0C: // RRC H
                            // code
                            break;
                        case 0x0D: // RRC L
                            // code
                            break;
                        case 0x0F: // RRC A
                            // code
                            break;
                        case 0x0E: // RRC (HL)
                            // code
                            break;
                        case 0x18: // RR B
                            // code
                            break;
                        case 0x19: // RR C
                            // code
                            break;
                        case 0x1A: // RR D
                            // code
                            break;
                        case 0x1B: // RR E
                            // code
                            break;
                        case 0x1C: // RR H
                            // code
                            break;
                        case 0x1D: // RR L
                            // code
                            break;
                        case 0x1F: // RR A
                            // code
                            break;
                        case 0x1E: // RR (HL)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x67: // RRD
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
                        case 0x0E: // RRC (IX+o)
                            // code
                            break;
                        case 0x1E: // RR (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0x0E: // RRC (IY+o)
                            // code
                            break;
                        case 0x1E: // RR (IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public RR()
        {
        }
    }
}

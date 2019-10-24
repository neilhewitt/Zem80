using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RLC : IInstructionImplementation
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
                        case 0x06: // RLC (IX+o)
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

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public RLC()
        {
        }
    }
}

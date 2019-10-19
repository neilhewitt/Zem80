using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SRL : IInstructionImplementation
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

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x38: // SRL B
                            // code
                            break;
                        case 0x39: // SRL C
                            // code
                            break;
                        case 0x3A: // SRL D
                            // code
                            break;
                        case 0x3B: // SRL E
                            // code
                            break;
                        case 0x3C: // SRL H
                            // code
                            break;
                        case 0x3D: // SRL L
                            // code
                            break;
                        case 0x3F: // SRL A
                            // code
                            break;
                        case 0x3E: // SRL (HL)
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
                        case 0x3E: // SRL (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0x3E: // SRL (IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public SRL()
        {
        }
    }
}

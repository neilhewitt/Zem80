using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SRA : IInstructionImplementation
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
                        case 0x28: // SRA B
                            // code
                            break;
                        case 0x29: // SRA C
                            // code
                            break;
                        case 0x2A: // SRA D
                            // code
                            break;
                        case 0x2B: // SRA E
                            // code
                            break;
                        case 0x2C: // SRA H
                            // code
                            break;
                        case 0x2D: // SRA L
                            // code
                            break;
                        case 0x2F: // SRA A
                            // code
                            break;
                        case 0x2E: // SRA (HL)
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
                        case 0x2E: // SRA (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0x2E: // SRA (IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public SRA()
        {
        }
    }
}

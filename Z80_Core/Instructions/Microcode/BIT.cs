using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class BIT : IInstructionImplementation
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

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {

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
                        case 0x46: // BIT 0,(IX+o)
                            // code
                            break;
                        case 0x4E: // BIT 1,(IX+o)
                            // code
                            break;
                        case 0x56: // BIT 2,(IX+o)
                            // code
                            break;
                        case 0x5E: // BIT 3,(IX+o)
                            // code
                            break;
                        case 0x66: // BIT 4,(IX+o)
                            // code
                            break;
                        case 0x6E: // BIT 5,(IX+o)
                            // code
                            break;
                        case 0x76: // BIT 6,(IX+o)
                            // code
                            break;
                        case 0x7E: // BIT 7,(IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0x46: // BIT 0,(IY+o)
                            // code
                            break;
                        case 0x4E: // BIT 1,(IY+o)
                            // code
                            break;
                        case 0x56: // BIT 2,(IY+o)
                            // code
                            break;
                        case 0x5E: // BIT 3,(IY+o)
                            // code
                            break;
                        case 0x66: // BIT 4,(IY+o)
                            // code
                            break;
                        case 0x6E: // BIT 5,(IY+o)
                            // code
                            break;
                        case 0x76: // BIT 6,(IY+o)
                            // code
                            break;
                        case 0x7E: // BIT 7,(IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }
    }
}

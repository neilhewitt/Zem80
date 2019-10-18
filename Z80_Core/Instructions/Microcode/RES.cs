using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RES : IInstructionImplementation
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
                        case 0x86: // RES 0,(IX+o)
                            // code
                            break;
                        case 0x8E: // RES 1,(IX+o)
                            // code
                            break;
                        case 0x96: // RES 2,(IX+o)
                            // code
                            break;
                        case 0x9E: // RES 3,(IX+o)
                            // code
                            break;
                        case 0xA6: // RES 4,(IX+o)
                            // code
                            break;
                        case 0xAE: // RES 5,(IX+o)
                            // code
                            break;
                        case 0xB6: // RES 6,(IX+o)
                            // code
                            break;
                        case 0xBE: // RES 7,(IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0x86: // RES 0,(IY+o)
                            // code
                            break;
                        case 0x8E: // RES 1,(IY+o)
                            // code
                            break;
                        case 0x96: // RES 2,(IY+o)
                            // code
                            break;
                        case 0x9E: // RES 3,(IY+o)
                            // code
                            break;
                        case 0xA6: // RES 4,(IY+o)
                            // code
                            break;
                        case 0xAE: // RES 5,(IY+o)
                            // code
                            break;
                        case 0xB6: // RES 6,(IY+o)
                            // code
                            break;
                        case 0xBE: // RES 7,(IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }
    }
}

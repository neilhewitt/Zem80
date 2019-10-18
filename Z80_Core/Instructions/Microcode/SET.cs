using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SET : IInstructionImplementation
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
                        case 0xC6: // SET 0,(IX+o)
                            // code
                            break;
                        case 0xCE: // SET 1,(IX+o)
                            // code
                            break;
                        case 0xD6: // SET 2,(IX+o)
                            // code
                            break;
                        case 0xDE: // SET 3,(IX+o)
                            // code
                            break;
                        case 0xE6: // SET 4,(IX+o)
                            // code
                            break;
                        case 0xEE: // SET 5,(IX+o)
                            // code
                            break;
                        case 0xF6: // SET 6,(IX+o)
                            // code
                            break;
                        case 0xFE: // SET 7,(IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0xC6: // SET 0,(IY+o)
                            // code
                            break;
                        case 0xCE: // SET 1,(IY+o)
                            // code
                            break;
                        case 0xD6: // SET 2,(IY+o)
                            // code
                            break;
                        case 0xDE: // SET 3,(IY+o)
                            // code
                            break;
                        case 0xE6: // SET 4,(IY+o)
                            // code
                            break;
                        case 0xEE: // SET 5,(IY+o)
                            // code
                            break;
                        case 0xF6: // SET 6,(IY+o)
                            // code
                            break;
                        case 0xFE: // SET 7,(IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class JR : IInstructionImplementation
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
                        case 0x18: // JR o
                            // code
                            break;
                        case 0x20: // JR NZ,o
                            // code
                            break;
                        case 0x28: // JR Z,o
                            // code
                            break;
                        case 0x30: // JR NC,o
                            // code
                            break;
                        case 0x38: // JR C,o
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x18: // JR o
                            // code
                            break;
                        case 0x20: // JR NZ,o
                            // code
                            break;
                        case 0x28: // JR Z,o
                            // code
                            break;
                        case 0x30: // JR NC,o
                            // code
                            break;
                        case 0x38: // JR C,o
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

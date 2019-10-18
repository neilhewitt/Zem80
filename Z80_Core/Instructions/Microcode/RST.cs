using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RST : IInstructionImplementation
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
                        case 0xC7: // RST 0
                            // code
                            break;
                        case 0xCF: // RST 8H
                            // code
                            break;
                        case 0xD7: // RST 10H
                            // code
                            break;
                        case 0xDF: // RST 18H
                            // code
                            break;
                        case 0xE7: // RST 20H
                            // code
                            break;
                        case 0xEF: // RST 28H
                            // code
                            break;
                        case 0xF7: // RST 30H
                            // code
                            break;
                        case 0xFF: // RST 38H
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0xC7: // RST 0
                            // code
                            break;
                        case 0xCF: // RST 8H
                            // code
                            break;
                        case 0xD7: // RST 10H
                            // code
                            break;
                        case 0xDF: // RST 18H
                            // code
                            break;
                        case 0xE7: // RST 20H
                            // code
                            break;
                        case 0xEF: // RST 28H
                            // code
                            break;
                        case 0xF7: // RST 30H
                            // code
                            break;
                        case 0xFF: // RST 38H
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

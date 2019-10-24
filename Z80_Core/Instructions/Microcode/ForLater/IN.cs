using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class IN : IInstructionImplementation
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
                        case 0xDB: // IN A,(n)
                            // code
                            break;

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
                        case 0x40: // IN B,(C)
                            // code
                            break;
                        case 0x48: // IN C,(C)
                            // code
                            break;
                        case 0x50: // IN D,(C)
                            // code
                            break;
                        case 0x58: // IN E,(C)
                            // code
                            break;
                        case 0x60: // IN H,(C)
                            // code
                            break;
                        case 0x68: // IN L,(C)
                            // code
                            break;
                        case 0x70: // IN F,(C)
                            // code
                            break;
                        case 0x78: // IN A,(C)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public IN()
        {
        }
    }
}

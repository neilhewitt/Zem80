using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CALL : IInstructionImplementation
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
                        case 0xC4: // CALL NZ,nn
                            // code
                            break;
                        case 0xCC: // CALL Z,nn
                            // code
                            break;
                        case 0xCD: // CALL nn
                            // code
                            break;
                        case 0xD4: // CALL NC,nn
                            // code
                            break;
                        case 0xDC: // CALL C,nn
                            // code
                            break;
                        case 0xE4: // CALL PO,nn
                            // code
                            break;
                        case 0xEC: // CALL PE,nn
                            // code
                            break;
                        case 0xF4: // CALL P,nn
                            // code
                            break;
                        case 0xFC: // CALL M,nn
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

        public CALL()
        {
        }
    }
}

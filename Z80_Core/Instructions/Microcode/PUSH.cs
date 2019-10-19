using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class PUSH : IInstructionImplementation
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
                        case 0xC5: // PUSH BC
                            // code
                            break;
                        case 0xD5: // PUSH DE
                            // code
                            break;
                        case 0xE5: // PUSH HL
                            // code
                            break;
                        case 0xF5: // PUSH AF
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
                        case 0xE5: // PUSH IX
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xE5: // PUSH IY
                            // code
                            break;

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

        public PUSH()
        {
        }
    }
}

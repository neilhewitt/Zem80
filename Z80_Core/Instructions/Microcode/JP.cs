using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class JP : IInstructionImplementation
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
                        case 0xC2: // JP NZ,nn
                            // code
                            break;
                        case 0xC3: // JP nn
                            // code
                            break;
                        case 0xCA: // JP Z,nn
                            // code
                            break;
                        case 0xD2: // JP NC,nn
                            // code
                            break;
                        case 0xDA: // JP C,nn
                            // code
                            break;
                        case 0xE2: // JP PO,nn
                            // code
                            break;
                        case 0xE9: // JP (HL)
                            // code
                            break;
                        case 0xEA: // JP PE,nn
                            // code
                            break;
                        case 0xF2: // JP P,nn
                            // code
                            break;
                        case 0xFA: // JP M,nn
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
                        case 0xE9: // JP (IX)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xE9: // JP (IY)
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

        public JP()
        {
        }
    }
}

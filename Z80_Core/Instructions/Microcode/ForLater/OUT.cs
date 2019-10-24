using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class OUT : IInstructionImplementation
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
                        case 0xD3: // OUT (n),A
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
                        case 0x41: // OUT (C),B
                            // code
                            break;
                        case 0x49: // OUT (C),C
                            // code
                            break;
                        case 0x51: // OUT (C),D
                            // code
                            break;
                        case 0x59: // OUT (C),E
                            // code
                            break;
                        case 0x61: // OUT (C),H
                            // code
                            break;
                        case 0x69: // OUT (C),L
                            // code
                            break;
                        case 0x79: // OUT (C),A
                            // code
                            break;
                        case 0xA3: // OUTI
                            // code
                            break;
                        case 0xAB: // OUTD
                            // code
                            break;

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

        public OUT()
        {
        }
    }
}

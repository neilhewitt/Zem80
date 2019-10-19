using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SUB : IInstructionImplementation
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
                        case 0x90: // SUB B
                            // code
                            break;
                        case 0x91: // SUB C
                            // code
                            break;
                        case 0x92: // SUB D
                            // code
                            break;
                        case 0x93: // SUB E
                            // code
                            break;
                        case 0x94: // SUB H
                            // code
                            break;
                        case 0x95: // SUB L
                            // code
                            break;
                        case 0x97: // SUB A
                            // code
                            break;
                        case 0x96: // SUB (HL)
                            // code
                            break;
                        case 0xD6: // SUB n
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
                        case 0x94: // SUB IXh
                            // code
                            break;
                        case 0x95: // SUB IXl
                            // code
                            break;
                        case 0x96: // SUB (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x94: // SUB IYh
                            // code
                            break;
                        case 0x95: // SUB IYl
                            // code
                            break;
                        case 0x96: // SUB (IY+o)
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

        public SUB()
        {
        }
    }
}

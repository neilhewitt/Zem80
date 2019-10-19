using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class XOR : IInstructionImplementation
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
                        case 0xA8: // XOR B
                            // code
                            break;
                        case 0xA9: // XOR C
                            // code
                            break;
                        case 0xAA: // XOR D
                            // code
                            break;
                        case 0xAB: // XOR E
                            // code
                            break;
                        case 0xAC: // XOR H
                            // code
                            break;
                        case 0xAD: // XOR L
                            // code
                            break;
                        case 0xAF: // XOR A
                            // code
                            break;
                        case 0xAE: // XOR (HL)
                            // code
                            break;
                        case 0xEE: // XOR n
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
                        case 0xAC: // XOR IXh
                            // code
                            break;
                        case 0xAD: // XOR IXl
                            // code
                            break;
                        case 0xAE: // XOR (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xAC: // XOR IYh
                            // code
                            break;
                        case 0xAD: // XOR IYl
                            // code
                            break;
                        case 0xAE: // XOR (IY+o)
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

        public XOR()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SLA : IInstructionImplementation
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

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x20: // SLA B
                            // code
                            break;
                        case 0x21: // SLA C
                            // code
                            break;
                        case 0x22: // SLA D
                            // code
                            break;
                        case 0x23: // SLA E
                            // code
                            break;
                        case 0x24: // SLA H
                            // code
                            break;
                        case 0x25: // SLA L
                            // code
                            break;
                        case 0x27: // SLA A
                            // code
                            break;
                        case 0x26: // SLA (HL)
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
                        case 0x26: // SLA (IX+o)
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {
                        case 0x26: // SLA (IY+o)
                            // code
                            break;

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public SLA()
        {
        }
    }
}

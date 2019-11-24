using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class POP : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            IStack s = cpu.Stack;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC1: // POP BC
                            r.BC = s.Pop();
                            break;
                        case 0xD1: // POP DE
                            r.DE = s.Pop();
                            break;
                        case 0xE1: // POP HL
                            r.HL = s.Pop();
                            break;
                        case 0xF1: // POP AF
                            r.AF = s.Pop();
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xE1: // POP IX
                            r.IX = s.Pop();
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xE1: // POP IY
                            r.IY = s.Pop();
                            break;

                    }
                    break;
            }

            return new ExecutionResult(package, cpu.Registers.Flags, false);
        }

        public POP()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class POP : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC1: // POP BC
                            cpu.Pop(RegisterWord.BC);
                            break;
                        case 0xD1: // POP DE
                            cpu.Pop(RegisterWord.DE);
                            break;
                        case 0xE1: // POP HL
                            cpu.Pop(RegisterWord.HL);
                            break;
                        case 0xF1: // POP AF
                            cpu.Pop(RegisterWord.AF);
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xE1: // POP IX
                            cpu.Pop(RegisterWord.IX);
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xE1: // POP IY
                            cpu.Pop(RegisterWord.IY);
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

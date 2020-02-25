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

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC1: // POP BC
                            cpu.Pop(RegisterPairName.BC);
                            break;
                        case 0xD1: // POP DE
                            cpu.Pop(RegisterPairName.DE);
                            break;
                        case 0xE1: // POP HL
                            cpu.Pop(RegisterPairName.HL);
                            break;
                        case 0xF1: // POP AF
                            cpu.Pop(RegisterPairName.AF);
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xE1: // POP IX
                            cpu.Pop(RegisterPairName.IX);
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xE1: // POP IY
                            cpu.Pop(RegisterPairName.IY);
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

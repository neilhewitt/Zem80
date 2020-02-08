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
            IRegisters r = cpu.Registers;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC5: // PUSH BC
                            cpu.Push(RegisterPairIndex.BC);
                            break;
                        case 0xD5: // PUSH DE
                            cpu.Push(RegisterPairIndex.DE);
                            break;
                        case 0xE5: // PUSH HL
                            cpu.Push(RegisterPairIndex.HL);
                            break;
                        case 0xF5: // PUSH AF
                            cpu.Push(RegisterPairIndex.AF);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xE5: // PUSH IX
                            cpu.Push(RegisterPairIndex.IX);
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xE5: // PUSH IY
                            cpu.Push(RegisterPairIndex.IY);
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, cpu.Registers.Flags, false);
        }

        public PUSH()
        {
        }
    }
}

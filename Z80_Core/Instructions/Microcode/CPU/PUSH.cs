using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class PUSH : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;   
            Registers r = cpu.Registers;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC5: // PUSH BC
                            cpu.Push(WordRegister.BC);
                            break;
                        case 0xD5: // PUSH DE
                            cpu.Push(WordRegister.DE);
                            break;
                        case 0xE5: // PUSH HL
                            cpu.Push(WordRegister.HL);
                            break;
                        case 0xF5: // PUSH AF
                            cpu.Push(WordRegister.AF);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xE5: // PUSH IX
                            cpu.Push(WordRegister.IX);
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xE5: // PUSH IY
                            cpu.Push(WordRegister.IY);
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, cpu.Registers.Flags, false, false);
        }

        public PUSH()
        {
        }
    }
}

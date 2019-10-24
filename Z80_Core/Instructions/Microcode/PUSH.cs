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
            Stack s = cpu.Stack;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC5: // PUSH BC
                            s.Push(r.BC);
                            break;
                        case 0xD5: // PUSH DE
                            s.Push(r.DE);
                            break;
                        case 0xE5: // PUSH HL
                            s.Push(r.HL);
                            break;
                        case 0xF5: // PUSH AF
                            s.Push(r.AF);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xE5: // PUSH IX
                            s.Push(r.IX);
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xE5: // PUSH IY
                            s.Push(r.IY);
                            break;
                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public PUSH()
        {
        }
    }
}

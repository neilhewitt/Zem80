using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class EX : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            ushort swap;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0x08: // EX AF,AF'
                            r.ExchangeAF();
                            break;
                        case 0xE3: // EX (SP),HL
                            swap = r.HL;
                            r.HL = cpu.Memory.ReadWordAt(r.SP);
                            cpu.Memory.WriteWordAt(r.SP, swap);
                            break;
                        case 0xEB: // EX DE,HL
                            swap = r.DE;
                            r.DE = r.HL;
                            r.HL = swap;
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xE3: // EX (SP),IX
                            swap = r.IX;
                            r.IX = cpu.Memory.ReadWordAt(r.SP);
                            cpu.Memory.WriteWordAt(r.SP, swap);
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xE3: // EX (SP),IY
                            swap = r.IY;
                            r.IY = cpu.Memory.ReadWordAt(r.SP);
                            cpu.Memory.WriteWordAt(r.SP, swap);
                            break;
                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }

        public EX()
        {
        }
    }
}

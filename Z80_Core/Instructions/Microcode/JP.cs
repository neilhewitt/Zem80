using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class JP : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IFlags flags = cpu.Registers.Flags;
            ushort address = data.ArgumentsAsWord;
            bool pcWasSet = false;

            void jp(ushort address)
            {
                cpu.Registers.PC = address;
                pcWasSet = true;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC2: // JP NZ,nn
                            if (!flags.Zero) jp(address);
                            break;
                        case 0xC3: // JP nn
                            jp(address);
                            break;
                        case 0xCA: // JP Z,nn
                            if (flags.Zero) jp(address);
                            break;
                        case 0xD2: // JP NC,nn
                            if (!flags.Carry) jp(address);
                            break;
                        case 0xDA: // JP C,nn
                            if (flags.Carry) jp(address);
                            break;
                        case 0xE2: // JP PO,nn
                            if (!flags.ParityOverflow) jp(address);
                            break;
                        case 0xE9: // JP (HL)
                            jp(cpu.Memory.ReadWordAt(cpu.Registers.HL));
                            break;
                        case 0xEA: // JP PE,nn
                            if (flags.ParityOverflow) jp(address);
                            break;
                        case 0xF2: // JP P,nn
                            if (!flags.Sign) jp(address);
                            break;
                        case 0xFA: // JP M,nn
                            if (flags.Sign) jp(address);
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, cpu.Registers.Flags, false, pcWasSet);
        }

        public JP()
        {
        }
    }
}

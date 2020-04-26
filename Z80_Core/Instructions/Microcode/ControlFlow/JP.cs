using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class JP : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            cpu.Registers.PC += instruction.SizeInBytes;

            void jp(ushort address)
            {
                cpu.Registers.PC = address;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xC2: // JP NZ,nn
                            if (!flags.Zero) jp(data.ArgumentsAsWord);
                            break;
                        case 0xC3: // JP nn
                            jp(data.ArgumentsAsWord);
                            break;
                        case 0xCA: // JP Z,nn
                            if (flags.Zero) jp(data.ArgumentsAsWord);
                            break;
                        case 0xD2: // JP NC,nn
                            if (!flags.Carry) jp(data.ArgumentsAsWord);
                            break;
                        case 0xDA: // JP C,nn
                            if (flags.Carry) jp(data.ArgumentsAsWord);
                            break;
                        case 0xE2: // JP PO,nn
                            if (!flags.ParityOverflow) jp(data.ArgumentsAsWord);
                            break;
                        case 0xE9: // JP HL
                            jp(cpu.Registers.HL);
                            break;
                        case 0xEA: // JP PE,nn
                            if (flags.ParityOverflow) jp(data.ArgumentsAsWord);
                            break;
                        case 0xF2: // JP P,nn
                            if (!flags.Sign) jp(data.ArgumentsAsWord);
                            break;
                        case 0xFA: // JP M,nn
                            if (flags.Sign) jp(data.ArgumentsAsWord);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xE9: // JP IX
                            jp(cpu.Registers.IX);
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xE9: // JP IY
                            jp(cpu.Registers.IY);
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, cpu.Registers.Flags, false, true);
        }

        public JP()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class OR : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;

            void or(byte operand)
            {
                r.A = (byte)(r.A | operand);
                if (((sbyte)r.A) < 0) flags.Sign = true;
                if (r.A == 0) flags.Zero = true;
                if (r.A.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xB0: // OR B
                            or(r.B);
                            break;
                        case 0xB1: // OR C
                            or(r.C);
                            break;
                        case 0xB2: // OR D
                            or(r.D);
                            break;
                        case 0xB3: // OR E
                            or(r.E);
                            break;
                        case 0xB4: // OR H
                            or(r.H);
                            break;
                        case 0xB5: // OR L
                            or(r.L);
                            break;
                        case 0xB7: // OR A
                            or(r.A);
                            break;
                        case 0xB6: // OR (HL)
                            or(cpu.Memory.ReadByteAt(r.HL));
                            break;
                        case 0xF6: // OR n
                            or(data.Argument1);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xB4: // OR IXh
                            or(r.IXh);
                            break;
                        case 0xB5: // OR IXl
                            or(r.IXl);
                            break;
                        case 0xB6: // OR (IX+o)
                            or(cpu.Memory.ReadByteAt((ushort)(r.IX + (sbyte)data.Argument1)));
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xB4: // OR IYh
                            or(r.IYh);
                            break;
                        case 0xB5: // OR IYl
                            or(r.IYl);
                            break;
                        case 0xB6: // OR (IY+o)
                            or(cpu.Memory.ReadByteAt((ushort)(r.IY + (sbyte)data.Argument1)));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(flags, 0);
        }

        public OR()
        {
        }
    }
}

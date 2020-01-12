using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class INC : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            byte offset = data.Argument1;
            Flags flags = new Flags();

            ushort incw(ushort value)
            {
                if (value < 0xFFFF) return (ushort)(value + 1);
                return 0;
            }

            byte inc(byte value)
            {
                flags.Carry = cpu.Registers.Flags.Carry;
                ushort result = (ushort)(value + 1);
                if (result == 0) flags.Zero = true;
                if (((sbyte)result) < 0) flags.Sign = true;
                if ((value & 0x0F) == 0x0F) flags.HalfCarry = true;
                if (value == 0x7F) flags.ParityOverflow = true;
                flags.Subtract = true;

                if (result > 0xFF) result = 0;
                return (byte)result;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0x03: // INC BC
                            r.BC = incw(r.BC);
                            break;
                        case 0x04: // INC B
                            r.B = inc(r.B);
                            break;
                        case 0x0C: // INC C
                            r.C = inc(r.C);
                            break;
                        case 0x13: // INC DE
                            r.DE = incw(r.DE);
                            break;
                        case 0x14: // INC D
                            r.D = inc(r.D);
                            break;
                        case 0x1C: // INC E
                            r.E = inc(r.E);
                            break;
                        case 0x23: // INC HL
                            r.HL = incw(r.HL);
                            break;
                        case 0x24: // INC H
                            r.H = inc(r.H);
                            break;
                        case 0x2C: // INC L
                            r.L = inc(r.L);
                            break;
                        case 0x33: // INC SP
                            r.SP = incw(r.SP);
                            break;
                        case 0x34: // INC (HL)
                            cpu.Memory.WriteByteAt(r.HL, inc(cpu.Memory.ReadByteAt(r.HL)));
                            break;
                        case 0x3C: // INC A
                            r.A = inc(r.A);
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x24: // INC IXh
                            r.IXh = inc(r.IXh);
                            break;
                        case 0x2C: // INC IXl
                            r.IXl = inc(r.IXl);
                            break;
                        case 0x23: // INC IX
                            r.IX = incw(r.IX);
                            break;
                        case 0x34: // INC (IX+o)
                            cpu.Memory.WriteByteAt((ushort)(r.IX + (sbyte)offset), inc(cpu.Memory.ReadByteAt((ushort)(r.IX + (sbyte)offset))));
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x24: // INC IYh
                            r.IYh = inc(r.IYh);
                            break;
                        case 0x2C: // INC IYl
                            r.IYl = inc(r.IYl);
                            break;
                        case 0x23: // INC IY
                            r.IY = incw(r.IY);
                            break;
                        case 0x34: // INC (IY+o)
                            cpu.Memory.WriteByteAt((ushort)(r.IY + (sbyte)offset), inc(cpu.Memory.ReadByteAt((ushort)(r.IY + (sbyte)offset))));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, flags, false);
        }

        public INC()
        {
        }
    }
}

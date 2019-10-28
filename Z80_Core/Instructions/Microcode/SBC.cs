using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SBC : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;

            byte rb(ushort address)
            {
                return cpu.Memory.ReadByteAt(address);
            }

            byte ro(ushort address, byte offset)
            {
                return cpu.Memory.ReadByteAt((ushort)(address + (sbyte)offset));
            }

            byte subc(byte value)
            {
                ushort result = (ushort)(cpu.Registers.A - value - (cpu.Registers.Flags.Carry ? 1 : 0));
                short signed = (short)result;
                if (result == 0) flags.Zero = true;
                if (result > 0xFF) flags.Carry = true;
                if (signed < 0) flags.Sign = true;
                if (signed > 0x7F || signed < -0x7F) flags.ParityOverflow = true;
                if ((((cpu.Registers.A & 0x0F) + (((byte)result) & 0x0F)) > 0x0F)) flags.HalfCarry = true;

                return (byte)result;
            }

            ushort subwc(ushort value)
            {
                uint result = (uint)(cpu.Registers.HL - value - (cpu.Registers.Flags.Carry ? 1 : 0));
                int signed = (int)result;
                if (result == 0) flags.Zero = true;
                if (result > 0xFFFF) flags.Carry = true;
                if (signed < 0) flags.Sign = true;
                if (signed > 0x7FFF || signed < -0x7FFF) flags.ParityOverflow = true;
                if ((cpu.Registers.HL & 0x0FFF) + (((byte)result) & 0x0FFF) > 0x0FFF) flags.HalfCarry = true;

                return (ushort)result;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0x98: // SBC A,B
                            r.A = subc(r.B);
                            break;
                        case 0x99: // SBC A,C
                            r.A = subc(r.C);
                            break;
                        case 0x9A: // SBC A,D
                            r.A = subc(r.D);
                            break;
                        case 0x9B: // SBC A,E
                            r.A = subc(r.E);
                            break;
                        case 0x9C: // SBC A,H
                            r.A = subc(r.H);
                            break;
                        case 0x9D: // SBC A,L
                            r.A = subc(r.L);
                            break;
                        case 0x9F: // SBC A,A
                            r.A = subc(r.A);
                            break;
                        case 0x9E: // SBC A,(HL)
                            r.A = subc(rb(r.HL));
                            break;
                        case 0xDE: // SBC A,n
                            r.A = subc(data.Argument1);
                            break;
                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x42: // SBC HL,BC
                            r.HL = subwc(r.BC);
                            break;
                        case 0x52: // SBC HL,DE
                            r.HL = subwc(r.DE);
                            break;
                        case 0x62: // SBC HL,HL
                            r.HL = subwc(r.HL);
                            break;
                        case 0x72: // SBC HL,SP
                            r.HL = subwc(r.SP);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x9C: // SBC A,IXh
                            r.A = subc(r.IXh);
                            break;
                        case 0x9D: // SBC A,IXl
                            r.A = subc(r.IXl);
                            break;
                        case 0x9E: // SBC A,(IX+o)
                            r.A = subc(ro(r.IX, data.Argument1));
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x9C: // SBC A,IYh
                            r.A = subc(r.IYh);
                            break;
                        case 0x9D: // SBC A,IYl
                            r.A = subc(r.IYl);
                            break;
                        case 0x9E: // SBC A,(IY+o)
                            r.A = subc(ro(r.IY, data.Argument1));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(flags, 0);
        }

        public SBC()
        {
        }
    }
}

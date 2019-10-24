using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ADD : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;

            byte readByte(ushort address)
            {
                return cpu.Memory.ReadByteAt(address);
            }

            byte readOffset(ushort address, byte offset)
            {
                return cpu.Memory.ReadByteAt((ushort)(address + offset));
            }

            byte addByte(byte value)
            {
                ushort result = (ushort)(cpu.Registers.A + value);
                short signed = (short)result;
                if (result == 0) flags.Zero = true;
                if (result > 0xFF) flags.Carry = true;
                if (signed < 0) flags.Sign = true;
                if (signed > 0x7F || signed < -0x7F) flags.ParityOverflow = true;
                if ((cpu.Registers.A & 0xF) + (((byte)result) & 0xF) > 0xF) flags.HalfCarry = true;

                return (byte)result;
            }

            ushort addWord(ushort first, ushort second)
            {
                flags = Flags.Copy(cpu.Registers.Flags); // must preserve existing values
                uint result = (uint)(first + second);
                if (result > 0xFFFF) flags.Carry = true;
                if ((first & 0x0FFF) + (((ushort)result) & 0x0FFF) > 0x0FFF) flags.HalfCarry = true;

                return (ushort)result;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0x09: // ADD HL,BC
                            r.HL = addWord(r.HL, r.BC);
                            break;
                        case 0x19: // ADD HL,DE
                            r.HL = addWord(r.HL, r.DE);
                            break;
                        case 0x29: // ADD HL,HL
                            r.HL = addWord(r.HL, r.HL);
                            break;
                        case 0x39: // ADD HL,SP
                            r.HL = addWord(r.HL, r.SP);
                            break;
                        case 0x80: // ADD A,B
                            r.A = addByte(r.B);
                            break;
                        case 0x81: // ADD A,C
                            r.A = addByte(r.C);
                            break;
                        case 0x82: // ADD A,D
                            r.A = addByte(r.D);
                            break;
                        case 0x83: // ADD A,E
                            r.A = addByte(r.E);
                            break;
                        case 0x84: // ADD A,H
                            r.A = addByte(r.H);
                            break;
                        case 0x85: // ADD A,L
                            r.A = addByte(r.L);
                            break;
                        case 0x87: // ADD A,A
                            r.A = addByte(r.A);
                            break;
                        case 0x86: // ADD A,(HL)
                            r.A = addByte(readByte(r.HL));
                            break;
                        case 0xC6: // ADD A,n
                            r.A = addByte(data.Arguments[0]);
                            break;
                    }
                    break;


                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x09: // ADD IX,BC
                            r.IX = addWord(r.IX, r.BC);
                            break;
                        case 0x19: // ADD IX,DE
                            r.IX = addWord(r.IX, r.DE);
                            break;
                        case 0x29: // ADD IX,IX
                            r.IX = addWord(r.IX, r.IX);
                            break;
                        case 0x39: // ADD IX,SP
                            r.IX = addWord(r.IX, r.SP);
                            break;
                        case 0x84: // ADD A,IXh
                            r.A = addByte(r.IXh);
                            break;
                        case 0x85: // ADD A,IXl
                            r.A = addByte(r.IXl);
                            break;
                        case 0x86: // ADD A,(IX+o)
                            r.A = addByte(readOffset(r.IX, data.Arguments[0]));
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x09: // ADD IY,BC
                            r.IY = addWord(r.IY, r.BC);
                            break;
                        case 0x19: // ADD IY,DE
                            r.IY = addWord(r.IY, r.DE);
                            break;
                        case 0x29: // ADD IY,IY
                            r.IY = addWord(r.IY, r.IY);
                            break;
                        case 0x39: // ADD IY,SP
                            r.IY = addWord(r.IY, r.SP);
                            break;
                        case 0x84: // ADD A,IYh
                            r.A = addByte(r.IYh);
                            break;
                        case 0x85: // ADD A,IYl
                            r.A = addByte(r.IYl);
                            break;
                        case 0x86: // ADD A,(IY+o)
                            r.A = addByte(readOffset(r.IY, data.Arguments[0]));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(flags, 0);
        }

        public ADD()
        {
        }
    }
}

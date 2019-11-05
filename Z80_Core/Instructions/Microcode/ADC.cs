using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ADC : IInstructionImplementation
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
                return cpu.Memory.ReadByteAt((ushort)(address + (sbyte)offset));
            }

            byte addByteWithCarry(byte value)
            {
                ushort result = (ushort)(cpu.Registers.A + value + (cpu.Registers.Flags.Carry ? 1 : 0));
                sbyte signed = (sbyte)((byte)result);
                if (result == 0) flags.Zero = true;
                if (result > 0xFF) flags.Carry = true;
                if (signed < 0) flags.Sign = true;
                if (signed > 0x7F || signed < -0x7F) flags.ParityOverflow = true;
                if ((((cpu.Registers.A & 0x0F) + (((byte)result) & 0x0F)) > 0x0F)) flags.HalfCarry = true;

                return (byte)result;
            }

            ushort addWordWithCarry(ushort value)
            {
                uint result = (uint)(cpu.Registers.HL + value + (cpu.Registers.Flags.Carry ? 1 : 0)); 
                int signed = (int)result;
                if (result == 0) flags.Zero = true;
                if (result > 0xFFFF) flags.Carry = true;
                if (signed < 0) flags.Sign = true;
                if (signed > 0x7FFF|| signed < -0x7FFF) flags.ParityOverflow = true;
                if ((cpu.Registers.HL & 0x0FFF) + (((byte)result) & 0x0FFF) > 0x0FFF) flags.HalfCarry = true;

                return (ushort)result;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0x88: // ADC A,B
                            r.A = addByteWithCarry(r.B);
                            break;
                        case 0x89: // ADC A,C
                            r.A = addByteWithCarry(r.C);
                            break;
                        case 0x8A: // ADC A,D
                            r.A = addByteWithCarry(r.D);
                            break;
                        case 0x8B: // ADC A,E
                            r.A = addByteWithCarry(r.E);
                            break;
                        case 0x8C: // ADC A,H
                            r.A = addByteWithCarry(r.H);
                            break;
                        case 0x8D: // ADC A,L
                            r.A = addByteWithCarry(r.L);
                            break;
                        case 0x8F: // ADC A,A
                            r.A = addByteWithCarry(r.A);
                            break;
                        case 0x8E: // ADC A,(HL)
                            r.A = addByteWithCarry(readByte(r.HL));
                            break;
                        case 0xCE: // ADC A,n
                            r.A = addByteWithCarry(data.Argument1);
                            break;
                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x4A: // ADC HL,BC
                            r.HL = addWordWithCarry(r.BC);
                            break;
                        case 0x5A: // ADC HL,DE
                            r.HL = addWordWithCarry(r.DE);
                            break;
                        case 0x6A: // ADC HL,HL
                            r.HL = addWordWithCarry(r.HL);
                            break;
                        case 0x7A: // ADC HL,SP
                            r.HL = addWordWithCarry(r.SP);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x8C: // ADC A,IXh
                            r.A = addByteWithCarry(r.IXh);
                            break;
                        case 0x8D: // ADC A,IXl
                            r.A = addByteWithCarry(r.IXl);
                            break;
                        case 0x8E: // ADC A,(IX+o)
                            r.A = addByteWithCarry(readOffset(r.IX, data.Argument1));
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x8C: // ADC A,IYh
                            r.A = addByteWithCarry(r.IYh);
                            break;
                        case 0x8D: // ADC A,IYl
                            r.A = addByteWithCarry(r.IYl);
                            break;
                        case 0x8E: // ADC A,(IY+o)
                            r.A = addByteWithCarry(readOffset(r.IY, data.Argument1));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, flags, false);
        }

        public ADC()
        {
        }
    }
}

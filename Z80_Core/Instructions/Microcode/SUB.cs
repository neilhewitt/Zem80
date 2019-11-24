using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SUB : IInstructionImplementation
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

            byte subByte(byte value)
            {
                ushort result = (ushort)(cpu.Registers.A - value);
                short signed = (short)result;
                if (result == 0) flags.Zero = true;
                if (result > 0xFF) flags.Carry = true;
                if ((sbyte)signed < 0) flags.Sign = true;
                if (signed > 0x7F || signed < -0x7F) flags.ParityOverflow = true;
                if ((cpu.Registers.A & 0x0F) + (((byte)result) & 0x0F) > 0x0F) flags.HalfCarry = true;

                return (byte)result;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0x90: // SUB B
                            r.A = subByte(r.B);
                            break;
                        case 0x91: // SUB C
                            r.A = subByte(r.C);
                            break;
                        case 0x92: // SUB D
                            r.A = subByte(r.D);
                            break;
                        case 0x93: // SUB E
                            r.A = subByte(r.E);
                            break;
                        case 0x94: // SUB H
                            r.A = subByte(r.H);
                            break;
                        case 0x95: // SUB L
                            r.A = subByte(r.L);
                            break;
                        case 0x97: // SUB A
                            r.A = subByte(r.A);
                            break;
                        case 0x96: // SUB (HL)
                            r.A = subByte(readByte(r.HL));
                            break;
                        case 0xD6: // SUB n
                            r.A = subByte(data.Argument1);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x94: // SUB IXh
                            r.A = subByte(r.IXh);
                            break;
                        case 0x95: // SUB IXl
                            r.A = subByte(r.IXl);
                            break;
                        case 0x96: // SUB (IX+o)
                            r.A = subByte(readByte((ushort)(r.IX + (sbyte)data.Argument1)));
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x94: // SUB IYh
                            r.A = subByte(r.IYh);
                            break;
                        case 0x95: // SUB IYl
                            r.A = subByte(r.IYl);
                            break;
                        case 0x96: // SUB (IY+o)
                            r.A = subByte(readByte((ushort)(r.IY + (sbyte)data.Argument1)));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, flags, false);
        }

        public SUB()
        {
        }
    }
}

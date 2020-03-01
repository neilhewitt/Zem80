using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class XOR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;

            void xor(byte operand)
            {
                int result = (byte)(r.A ^ operand);
                FlagHelper.SetFlagsFromLogicalOperation(flags, r.A, operand, result);
                r.A = (byte)result;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xA8: // XOR B
                            xor(r.B);
                            break;
                        case 0xA9: // XOR C
                            xor(r.C);
                            break;
                        case 0xAA: // XOR D
                            xor(r.D);
                            break;
                        case 0xAB: // XOR E
                            xor(r.E);
                            break;
                        case 0xAC: // XOR H
                            xor(r.H);
                            break;
                        case 0xAD: // XOR L
                            xor(r.L);
                            break;
                        case 0xAF: // XOR A
                            xor(r.A);
                            break;
                        case 0xAE: // XOR (HL)
                            xor(cpu.Memory.ReadByteAt(r.HL));
                            break;
                        case 0xEE: // XOR n
                            xor(data.Argument1);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xAC: // XOR IXh
                            xor(r.IXh);
                            break;
                        case 0xAD: // XOR IXl
                            xor(r.IXl);
                            break;
                        case 0xAE: // XOR (IX+o)
                            xor(cpu.Memory.ReadByteAt((ushort)(r.IX + (sbyte)data.Argument1)));
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xAC: // YOR IYh
                            xor(r.IYh);
                            break;
                        case 0xAD: // YOR IYl
                            xor(r.IYl);
                            break;
                        case 0xAE: // YOR (IY+o)
                            xor(cpu.Memory.ReadByteAt((ushort)(r.IY + (sbyte)data.Argument1)));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, flags, false);
        }

        public XOR()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SBC : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            byte readByte(ushort address)
            {
                return cpu.Memory.ReadByteAt(address);
            }

            byte readOffset(ushort address, byte offset)
            {
                return cpu.Memory.ReadByteAt((ushort)(address + (sbyte)offset));
            }

            byte subByteWithCarry(byte value)
            {
                int result = cpu.Registers.A - value - (flags.Carry ? 1 : 0);
                flags = FlagLookup.ByteArithmeticFlags(cpu.Registers.A, value, flags.Carry, true);
                return (byte)result;
            }

            ushort subWordWithCarry(ushort value)
            {
                int result = cpu.Registers.HL - value - (flags.Carry ? 1 : 0);
                flags = FlagLookup.WordArithmeticFlags(flags, cpu.Registers.HL, value, flags.Carry, true, true);
                return (ushort)result;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0x98: // SBC A,B
                            r.A = subByteWithCarry(r.B);
                            break;
                        case 0x99: // SBC A,C
                            r.A = subByteWithCarry(r.C);
                            break;
                        case 0x9A: // SBC A,D
                            r.A = subByteWithCarry(r.D);
                            break;
                        case 0x9B: // SBC A,E
                            r.A = subByteWithCarry(r.E);
                            break;
                        case 0x9C: // SBC A,H
                            r.A = subByteWithCarry(r.H);
                            break;
                        case 0x9D: // SBC A,L
                            r.A = subByteWithCarry(r.L);
                            break;
                        case 0x9F: // SBC A,A
                            r.A = subByteWithCarry(r.A);
                            break;
                        case 0x9E: // SBC A,(HL)
                            r.A = subByteWithCarry(readByte(r.HL));
                            break;
                        case 0xDE: // SBC A,n
                            r.A = subByteWithCarry(data.Argument1);
                            break;
                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x42: // SBC HL,BC
                            cpu.InternalOperationCycle(4);
                            cpu.InternalOperationCycle(3);
                            r.HL = subWordWithCarry(r.BC);
                            break;
                        case 0x52: // SBC HL,DE
                            cpu.InternalOperationCycle(4);
                            cpu.InternalOperationCycle(3);
                            r.HL = subWordWithCarry(r.DE);
                            break;
                        case 0x62: // SBC HL,HL
                            cpu.InternalOperationCycle(4);
                            cpu.InternalOperationCycle(3);
                            r.HL = subWordWithCarry(r.HL);
                            break;
                        case 0x72: // SBC HL,SP
                            cpu.InternalOperationCycle(4);
                            cpu.InternalOperationCycle(3);
                            r.HL = subWordWithCarry(r.SP);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x9C: // SBC A,IXh
                            r.A = subByteWithCarry(r.IXh);
                            break;
                        case 0x9D: // SBC A,IXl
                            r.A = subByteWithCarry(r.IXl);
                            break;
                        case 0x9E: // SBC A,(IX+o)
                            cpu.InternalOperationCycle(5);
                            r.A = subByteWithCarry(readOffset(r.IX, data.Argument1));
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x9C: // SBC A,IYh
                            r.A = subByteWithCarry(r.IYh);
                            break;
                        case 0x9D: // SBC A,IYl
                            r.A = subByteWithCarry(r.IYl);
                            break;
                        case 0x9E: // SBC A,(IY+o)
                            cpu.InternalOperationCycle(5);
                            r.A = subByteWithCarry(readOffset(r.IY, data.Argument1));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, flags, false, false);
        }

        public SBC()
        {
        }
    }
}

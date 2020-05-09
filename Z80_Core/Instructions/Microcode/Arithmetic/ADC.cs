using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ADC : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            byte readByte(ushort address)
            {
                return cpu.Memory.ReadByteAt(address, false);
            }

            byte readOffset(ushort address, sbyte offset)
            {
                return cpu.Memory.ReadByteAt((ushort)(address + offset), false);
            }

            byte addByteWithCarry(int value)
            {
                int result = cpu.Registers.A + value + (flags.Carry ? 1 : 0);
                flags = FlagLookup.ByteArithmeticFlags(cpu.Registers.A, value, flags.Carry, false);
                return (byte)result;
            }

            ushort addWordWithCarry(int value)
            {
                int result = cpu.Registers.HL + value + (flags.Carry ? 1 : 0);
                flags = FlagLookup.WordArithmeticFlags(flags, cpu.Registers.HL, value, flags.Carry, true, false);
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
                            cpu.InternalOperationCycle(4);
                            cpu.InternalOperationCycle(3);
                            r.HL = addWordWithCarry(r.BC);
                            break;
                        case 0x5A: // ADC HL,DE
                            cpu.InternalOperationCycle(4);
                            cpu.InternalOperationCycle(3);
                            r.HL = addWordWithCarry(r.DE);
                            break;
                        case 0x6A: // ADC HL,HL                            
                            cpu.InternalOperationCycle(4);
                            cpu.InternalOperationCycle(3);
                            r.HL = addWordWithCarry(r.HL);
                            break;
                        case 0x7A: // ADC HL,SP
                            cpu.InternalOperationCycle(4);
                            cpu.InternalOperationCycle(3);
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
                            cpu.InternalOperationCycle(5);
                            r.A = addByteWithCarry(readOffset(r.IX, (sbyte)data.Argument1));
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
                            cpu.InternalOperationCycle(5);
                            r.A = addByteWithCarry(readOffset(r.IY, (sbyte)data.Argument1));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, flags, false, false);
        }

        public ADC()
        {
        }
    }
}

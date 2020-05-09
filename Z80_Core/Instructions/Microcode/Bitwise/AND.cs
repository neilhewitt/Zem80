using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class AND : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            Flags flags = cpu.Registers.Flags;
            sbyte offset = (sbyte)data.Argument1;

            byte and(byte operand)
            {
                int result = (r.A & operand);
                flags = FlagLookup.LogicalFlags(r.A, operand, LogicalOperation.And);
                return (byte)result;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xA0: // AND B
                            r.A = and(r.B);
                            break;
                        case 0xA1: // AND C
                            r.A = and(r.C);
                            break;
                        case 0xA2: // AND D
                            r.A = and(r.D);
                            break;
                        case 0xA3: // AND E
                            r.A = and(r.E);
                            break;
                        case 0xA4: // AND H
                            r.A = and(r.H);
                            break;
                        case 0xA5: // AND L
                            r.A = and(r.L);
                            break;
                        case 0xA7: // AND A
                            r.A = and(r.A);
                            break;
                        case 0xA6: // AND (HL)
                            r.A = and(cpu.Memory.ReadByteAt(r.HL, false));
                            break;
                        case 0xE6: // AND n
                            r.A = and(data.Argument1);
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xA4: // AND IXh
                            r.A = and(r.IXh);
                            break;
                        case 0xA5: // AND IXl
                            r.A = and(r.IXl);
                            break;
                        case 0xA6: // AND (IX+o)
                            cpu.InternalOperationCycle(5);
                            r.A = and(cpu.Memory.ReadByteAt((ushort)(r.IX + offset), false));
                            break;
                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xA4: // AND IYh
                            r.A = and(r.IYh);
                            break;
                        case 0xA5: // AND IYl
                            r.A = and(r.IYl);
                            break;
                        case 0xA6: // AND (IY+o)
                            cpu.InternalOperationCycle(5);
                            r.A = and(cpu.Memory.ReadByteAt((ushort)(r.IY + offset), false));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, flags, false, false);
        }

        public AND()
        {
        }
    }
}

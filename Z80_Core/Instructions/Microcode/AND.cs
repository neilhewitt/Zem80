using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class AND : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            Flags flags = new Flags();

            byte and(byte operand)
            {
                byte result = (byte)(r.A & operand);

                if (result == 0x00) flags.Zero = true;
                if (((sbyte)result) < 0) flags.Sign = true;
                if (result.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
                flags.HalfCarry = true;

                return result;
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
                            r.A = and(cpu.Memory.ReadByteAt(r.HL));
                            break;
                        case 0xE6: // AND n
                            r.A = and(data.Arguments[0]);
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
                            r.A = and(cpu.Memory.ReadByteAt((ushort)(r.IX + data.Arguments[0])));
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
                            r.A = and(cpu.Memory.ReadByteAt((ushort)(r.IY + data.Arguments[0])));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(flags, 0);
        }

        public AND()
        {
        }
    }
}

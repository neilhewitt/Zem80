using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CP : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            Flags flags = new Flags();

            void cp(byte value)
            {
                byte a = cpu.Registers.A;
                byte b = value;
                short result = (short)(a - b); // note signed short - contains overflow, caters for negative results
                if (result == 0) flags.Zero = true;
                if ((sbyte)result < 0) flags.Sign = true;
                if ((a & 0x0F) < (b & 0x0F)) flags.HalfCarry = true;
                if (result > 0x7F || result < -0x80) flags.ParityOverflow = true;
                if (result > 0xFF) flags.Carry = true;
                flags.Subtract = true;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0xB8: // CP B
                            cp(r.B);
                            break;
                        case 0xB9: // CP C
                            cp(r.C);
                            break;
                        case 0xBA: // CP D
                            cp(r.D);
                            break;
                        case 0xBB: // CP E
                            cp(r.E);
                            break;
                        case 0xBC: // CP H
                            cp(r.H);
                            break;
                        case 0xBD: // CP L
                            cp(r.L);
                            break;
                        case 0xBF: // CP A
                            cp(r.A);
                            break;
                        case 0xBE: // CP (HL)
                            cp(cpu.Memory.ReadByteAt(r.HL));
                            break;
                        case 0xFE: // CP n
                            cp(data.Argument1);
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0xBC: // CP IXh
                            cp(r.IXh);
                            break;
                        case 0xBD: // CP IXl
                            cp(r.IXl);
                            break;
                        case 0xBE: // CP (IX+o)
                            cp(cpu.Memory.ReadByteAt((ushort)(r.IX + data.Argument1)));
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0xBC: // CP IYh
                            cp(r.IYh);
                            break;
                        case 0xBD: // CP IYl
                            cp(r.IYl);
                            break;
                        case 0xBE: // CP (IY+o)
                            cp(cpu.Memory.ReadByteAt((ushort)(r.IY + data.Argument1)));
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, flags, false);
        }

        public CP()
        {
        }
    }
}

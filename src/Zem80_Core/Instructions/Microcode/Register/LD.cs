using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class LD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            Registers r = cpu.Registers;
            byte arg0 = data.Argument1;
            byte arg1 = data.Argument2;
            ushort argWord = data.ArgumentsAsWord;
            
            //local functions to keep code size down
            byte readByte(ushort address)
            {
                return cpu.Memory.ReadByteAt(address, false);
            }

            ushort readWord(ushort address)
            {
                return cpu.Memory.ReadWordAt(address, false);
            }

            byte readOffset(ushort address, byte offset)
            {
                address = (ushort)(address + (sbyte)offset);
                byte value = cpu.Memory.ReadByteAt(address, false);
                flags.X = (address & 0x08) > 0; // copy bit 3
                flags.Y = (address & 0x20) > 0; // copy bit 5
                return value;
            }

            void writeByte(ushort address, byte value)
            {
                cpu.Memory.WriteByteAt(address, value, false);
            }

            void writeWord(ushort address, ushort value)
            {
                cpu.Memory.WriteWordAt(address, value, false);
            }

            void writeOffset(ushort address, byte offset, byte value)
            {
                cpu.Memory.WriteByteAt((ushort)(address + (sbyte)offset), value, false);
            }

            void handleIRFlags(byte input)
            {
                flags.Zero = (input == 0x00);
                flags.Sign = ((sbyte)input < 0);
                flags.ParityOverflow = (cpu.InterruptMode != InterruptMode.IM0);
                flags.X = (input & 0x08) > 0; // copy bit 3
                flags.Y = (input & 0x20) > 0; // copy bit 5
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0x01: // LD BC,nn
                            r.BC = argWord;
                            break;
                        case 0x02: // LD (BC),A
                            writeByte(r.BC, r.A);
                            break;
                        case 0x06: // LD B,n
                            r.B = arg0;
                            break;
                        case 0x0A: // LD A,(BC)
                            r.A = readByte(r.BC);
                            break;
                        case 0x0E: // LD C,n
                            r.C = arg0;
                            break;
                        case 0x11: // LD DE,nn
                            r.DE = argWord;
                            break;
                        case 0x12: // LD (DE),A
                            writeByte(r.DE, r.A);
                            break;
                        case 0x16: // LD D,n
                            r.D = arg0;
                            break;
                        case 0x1A: // LD A,(DE)
                            r.A = readByte(r.DE);
                            break;
                        case 0x1E: // LD E,n
                            r.E = arg0;
                            break;
                        case 0x21: // LD HL,nn
                            r.HL = argWord;
                            break;
                        case 0x22: // LD (nn),HL
                            writeWord(argWord, r.HL);
                            break;
                        case 0x26: // LD H,n
                            r.H = arg0;
                            break;
                        case 0x2A: // LD HL,(nn)
                            r.HL = readWord(argWord);
                            break;
                        case 0x2E: // LD L,n
                            r.L = arg0;
                            break;
                        case 0x31: // LD SP,nn
                            r.SP = argWord;
                            break;
                        case 0x32: // LD (nn),A
                            writeByte(argWord, r.A);
                            break;
                        case 0x36: // LD (HL),n
                            writeByte(r.HL, arg0);
                            break;
                        case 0x3A: // LD A,(nn)
                            r.A = readByte(argWord);
                            break;
                        case 0x3E: // LD A,n
                            r.A = arg0;
                            break;
                        case 0x40: // LD B,B
                            r.B = r.B; // yes, we have to do this, it's not just a NOP
                            break;
                        case 0x41: // LD B,C
                            r.B = r.C;
                            break;
                        case 0x42: // LD B,D
                            r.B = r.D;
                            break;
                        case 0x43: // LD B,E
                            r.B = r.E;
                            break;
                        case 0x44: // LD B,H
                            r.B = r.H;
                            break;
                        case 0x45: // LD B,L
                            r.B = r.L;
                            break;
                        case 0x47: // LD B,A
                            r.B = r.A;
                            break;
                        case 0x46: // LD B,(HL)
                            r.B = readByte(r.HL);
                            break;
                        case 0x48: // LD C,B
                            r.C = r.B;
                            break;
                        case 0x49: // LD C,C
                            r.C = r.C;
                            break;
                        case 0x4A: // LD C,D
                            r.C = r.D;
                            break;
                        case 0x4B: // LD C,E
                            r.C = r.E;
                            break;
                        case 0x4C: // LD C,H
                            r.C = r.H;
                            break;
                        case 0x4D: // LD C,L
                            r.C = r.L;
                            break;
                        case 0x4F: // LD C,A
                            r.C = r.A;
                            break;
                        case 0x4E: // LD C,(HL)
                            r.C = readByte(r.HL);
                            break;
                        case 0x50: // LD D,B
                            r.D = r.B;
                            break;
                        case 0x51: // LD D,C
                            r.D = r.C;
                            break;
                        case 0x52: // LD D,D
                            r.D = r.D;
                            break;
                        case 0x53: // LD D,E
                            r.D = r.E;
                            break;
                        case 0x54: // LD D,H
                            r.D = r.H;
                            break;
                        case 0x55: // LD D,L
                            r.D = r.L;
                            break;
                        case 0x57: // LD D,A
                            r.D = r.A;
                            break;
                        case 0x56: // LD D,(HL)
                            r.D = readByte(r.HL);
                            break;
                        case 0x58: // LD E,B
                            r.E = r.B;
                            break;
                        case 0x59: // LD E,C
                            r.E = r.C;
                            break;
                        case 0x5A: // LD E,D
                            r.E = r.D;
                            break;
                        case 0x5B: // LD E,E
                            r.E = r.E;
                            break;
                        case 0x5C: // LD E,H
                            r.E = r.H;
                            break;
                        case 0x5D: // LD E,L
                            r.E = r.L;
                            break;
                        case 0x5F: // LD E,A
                            r.E = r.A;
                            break;
                        case 0x5E: // LD E,(HL)
                            r.E = readByte(r.HL);
                            break;
                        case 0x60: // LD H,B
                            r.H = r.B;
                            break;
                        case 0x61: // LD H,C
                            r.H = r.C;
                            break;
                        case 0x62: // LD H,D
                            r.H = r.D;
                            break;
                        case 0x63: // LD H,E
                            r.H = r.E;
                            break;
                        case 0x64: // LD H,H
                            r.H = r.H;
                            break;
                        case 0x65: // LD H,L
                            r.H = r.L;
                            break;
                        case 0x67: // LD H,A
                            r.H = r.A;
                            break;
                        case 0x66: // LD H,(HL)
                            r.H = readByte(r.HL);
                            break;
                        case 0x68: // LD L,B
                            r.L = r.B;
                            break;
                        case 0x69: // LD L,C
                            r.L = r.C;
                            break;
                        case 0x6A: // LD L,D
                            r.L = r.D;
                            break;
                        case 0x6B: // LD L,E
                            r.L = r.E;
                            break;
                        case 0x6C: // LD L,H
                            r.L = r.H;
                            break;
                        case 0x6D: // LD L,L
                            r.L = r.L;
                            break;
                        case 0x6F: // LD L,A
                            r.L = r.A;
                            break;
                        case 0x6E: // LD L,(HL)
                            r.L = readByte(r.HL);
                            break;
                        case 0x70: // LD (HL),B
                            writeByte(r.HL, r.B);
                            break;
                        case 0x71: // LD (HL),C
                            writeByte(r.HL, r.C);
                            break;
                        case 0x72: // LD (HL),D
                            writeByte(r.HL, r.D);
                            break;
                        case 0x73: // LD (HL),E
                            writeByte(r.HL, r.E);
                            break;
                        case 0x74: // LD (HL),H
                            writeByte(r.HL, r.H);
                            break;
                        case 0x75: // LD (HL),L
                            writeByte(r.HL, r.L);
                            break;
                        case 0x77: // LD (HL),A
                            writeByte(r.HL, r.A);
                            break;
                        case 0x78: // LD A,B
                            r.A = r.B;
                            break;
                        case 0x79: // LD A,C
                            r.A = r.C;
                            break;
                        case 0x7A: // LD A,D
                            r.A = r.D;
                            break;
                        case 0x7B: // LD A,E
                            r.A = r.E;
                            break;
                        case 0x7C: // LD A,H
                            r.A = r.H;
                            break;
                        case 0x7D: // LD A,L
                            r.A = r.L;
                            break;
                        case 0x7F: // LD A,A
                            r.A = r.A;
                            break;
                        case 0x7E: // LD A,(HL)
                            r.A = readByte(r.HL);
                            break;
                        case 0xF9: // LD SP,HL
                            r.SP = r.HL;
                            break;
                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x43: // LD (nn),BC
                            writeWord(argWord, r.BC);
                            break;
                        case 0x47: // LD I,A
                            r.I = r.A;
                            handleIRFlags(r.A);
                            break;
                        case 0x4B: // LD BC,(nn)
                            r.BC = readWord(argWord);
                            break;
                        case 0x4F: // LD R,A
                            r.R = r.A;
                            handleIRFlags(r.A);
                            break;
                        case 0x53: // LD (nn),DE
                            writeWord(argWord, r.DE);
                            break;
                        case 0x57: // LD A,I
                            r.A = r.I;
                            handleIRFlags(r.A);
                            break;
                        case 0x5B: // LD DE,(nn)
                            r.DE = readWord(argWord);
                            break;
                        case 0x5F: // LD A,R
                            r.A = r.R;
                            handleIRFlags(r.A);
                            break;
                        case 0x73: // LD (nn),SP
                            writeWord(argWord, r.SP);
                            break;
                        case 0x7B: // LD SP,(nn)
                            r.SP = readWord(argWord);
                            break;
                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x21: // LD IX,nn
                            r.IX = argWord;
                            break;
                        case 0x22: // LD (nn),IX
                            writeWord(argWord, r.IX);
                            break;
                        case 0x26: // LD IXh,n
                            r.IXh = arg0;
                            break;
                        case 0x2A: // LD IX,(nn)
                            r.IX = readWord(argWord);
                            break;
                        case 0x2E: // LD IXl,n
                            r.IXl = arg0;
                            break;
                        case 0x36: // LD (IX+o),n
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IX, arg0, arg1);
                            break;
                        case 0x44: // LD B,IXh
                            r.B = r.IXh;
                            break;
                        case 0x45: // LD B,IXl
                            r.B = r.IXl;
                            break;
                        case 0x46: // LD B,(IX+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.B = readOffset(r.IX, arg0);
                            break;
                        case 0x4C: // LD C,IXh
                            r.C = r.IXh;
                            break;
                        case 0x4D: // LD C,IXl
                            r.C = r.IXl;
                            break;
                        case 0x4E: // LD C,(IX+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.C = readOffset(r.IX, arg0);
                            break;
                        case 0x54: // LD D,IXh
                            r.D = r.IXh;
                            break;
                        case 0x55: // LD D,IXl
                            r.D = r.IXl;
                            break;
                        case 0x56: // LD D,(IX+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.D = readOffset(r.IX, arg0);
                            break;
                        case 0x5C: // LD E,IXh
                            r.E = r.IXh;
                            break;
                        case 0x5D: // LD E,IXl
                            r.E = r.IXl;
                            break;
                        case 0x5E: // LD E,(IX+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.E = readOffset(r.IX, arg0);
                            break;
                        case 0x60: // LD IXh,B
                            r.IXh = r.B;
                            break;
                        case 0x61: // LD IXh,C
                            r.IXh = r.C;
                            break;
                        case 0x62: // LD IXh,D
                            r.IXh = r.D;
                            break;
                        case 0x63: // LD IXh,E
                            r.IXh = r.E;
                            break;
                        case 0x64: // LD IXh,IXh
                            r.IXh = r.IXh;
                            break;
                        case 0x65: // LD IXh,IXl
                            r.IXh = r.IXl;
                            break;
                        case 0x67: // LD IXh,A
                            r.IXh = r.A;
                            break;
                        case 0x66: // LD H,(IX+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.H = readOffset(r.IX, arg0);
                            break;
                        case 0x68: // LD IXl,B
                            r.IXl = r.B;
                            break;
                        case 0x69: // LD IXl,C
                            r.IXl = r.C;
                            break;
                        case 0x6A: // LD IXl,D
                            r.IXl = r.D;
                            break;
                        case 0x6B: // LD IXl,E
                            r.IXl = r.E;
                            break;
                        case 0x6C: // LD IXl,IXh
                            r.IXl = r.IXh;
                            break;
                        case 0x6D: // LD IXl,IXl
                            r.IXl = r.IXl;
                            break;
                        case 0x6F: // LD IXl,A
                            r.IXl = r.A;
                            break;
                        case 0x6E: // LD L,(IX+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.L = readOffset(r.IX, arg0);
                            break;
                        case 0x70: // LD (IX+o),B
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IX, arg0, r.B);
                            break;
                        case 0x71: // LD (IX+o),C
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IX, arg0, r.C);
                            break;
                        case 0x72: // LD (IX+o),D
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IX, arg0, r.D);
                            break;
                        case 0x73: // LD (IX+o),E
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IX, arg0, r.E);
                            break;
                        case 0x74: // LD (IX+o),H
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IX, arg0, r.H);
                            break;
                        case 0x75: // LD (IX+o),L
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IX, arg0, r.L);
                            break;
                        case 0x77: // LD (IX+o),A
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IX, arg0, r.A);
                            break;
                        case 0x7C: // LD A,IXh
                            r.A = r.IXh;
                            break;
                        case 0x7D: // LD A,IXl
                            r.A = r.IXl;
                            break;
                        case 0x7E: // LD A,(IX+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.A = readOffset(r.IX, arg0);
                            break;
                        case 0xF9: // LD SP,IX
                            r.SP = r.IX;
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x21: // LD IY,nn
                            r.IY = argWord;
                            break;
                        case 0x22: // LD (nn),IY
                            writeWord(argWord, r.IY);
                            break;
                        case 0x26: // LD IYh,n
                            r.IYh = arg0;
                            break;
                        case 0x2A: // LD IY,(nn)
                            r.IY = readWord(argWord);
                            break;
                        case 0x2E: // LD IYl,n
                            r.IYl = arg0;
                            break;
                        case 0x36: // LD (IY+o),n
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IY, arg0, arg1);
                            break;
                        case 0x44: // LD B,IYh
                            r.B = r.IYh;
                            break;
                        case 0x45: // LD B,IYl
                            r.B = r.IYl;
                            break;
                        case 0x46: // LD B,(IY+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.B = readOffset(r.IY, arg0);
                            break;
                        case 0x4C: // LD C,IYh
                            r.C = r.IYh;
                            break;
                        case 0x4D: // LD C,IYl
                            r.C = r.IYl;
                            break;
                        case 0x4E: // LD C,(IY+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.C = readOffset(r.IY, arg0);
                            break;
                        case 0x54: // LD D,IYh
                            r.D = r.IYh;
                            break;
                        case 0x55: // LD D,IYl
                            r.D = r.IYl;
                            break;
                        case 0x56: // LD D,(IY+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.D = readOffset(r.IY, arg0);
                            break;
                        case 0x5C: // LD E,IYh
                            r.E = r.IYh;
                            break;
                        case 0x5D: // LD E,IYl
                            r.E = r.IYl;
                            break;
                        case 0x5E: // LD E,(IY+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.E = readOffset(r.IY, arg0);
                            break;
                        case 0x60: // LD IYh,B
                            r.IYh = r.B;
                            break;
                        case 0x61: // LD IYh,C
                            r.IYh = r.C;
                            break;
                        case 0x62: // LD IYh,D
                            r.IYh = r.D;
                            break;
                        case 0x63: // LD IYh,E
                            r.IYh = r.E;
                            break;
                        case 0x64: // LD IYh,IYh
                            r.IYh = r.IYh;
                            break;
                        case 0x65: // LD IYh,IYl
                            r.IYh = r.IYl;
                            break;
                        case 0x67: // LD IYh,A
                            r.IYh = r.A;
                            break;
                        case 0x66: // LD H,(IY+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.H = readOffset(r.IY, arg0);
                            break;
                        case 0x68: // LD IYl,B
                            r.IYl = r.B;
                            break;
                        case 0x69: // LD IYl,C
                            r.IYl = r.C;
                            break;
                        case 0x6A: // LD IYl,D
                            r.IYl = r.D;
                            break;
                        case 0x6B: // LD IYl,E
                            r.IYl = r.E;
                            break;
                        case 0x6C: // LD IYl,IYh
                            r.IYl = r.IYh;
                            break;
                        case 0x6D: // LD IYl,IYl
                            r.IYl = r.IYl;
                            break;
                        case 0x6F: // LD IYl,A
                            r.IYl = r.A;
                            break;
                        case 0x6E: // LD L,(IY+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.L = readOffset(r.IY, arg0);
                            break;
                        case 0x70: // LD (IY+o),B
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IY, arg0, r.B);
                            break;
                        case 0x71: // LD (IY+o),C
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IY, arg0, r.C);
                            break;
                        case 0x72: // LD (IY+o),D
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IY, arg0, r.D);
                            break;
                        case 0x73: // LD (IY+o),E
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IY, arg0, r.E);
                            break;
                        case 0x74: // LD (IY+o),H
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IY, arg0, r.H);
                            break;
                        case 0x75: // LD (IY+o),L
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IY, arg0, r.L);
                            break;
                        case 0x77: // LD (IY+o),A
                            cpu.Timing.InternalOperationCycle(5);
                            writeOffset(r.IY, arg0, r.A);
                            break;
                        case 0x7C: // LD A,IYh
                            r.A = r.IYh;
                            break;
                        case 0x7D: // LD A,IYl
                            r.A = r.IYl;
                            break;
                        case 0x7E: // LD A,(IY+o)
                            cpu.Timing.InternalOperationCycle(5);
                            r.A = readOffset(r.IY, arg0);
                            break;
                        case 0xF9: // LD SP,IY
                            r.SP = r.IY;
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, flags);
        }

        public LD()
        {
        }
    }
}

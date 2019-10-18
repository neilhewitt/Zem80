using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class LD : IInstructionImplementation
    {
        public ExecutionResult Execute(InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0x01: // LD BC,nn
                            // code
                            break;
                        case 0x02: // LD (BC),A
                            // code
                            break;
                        case 0x06: // LD B,n
                            // code
                            break;
                        case 0x0A: // LD A,(BC)
                            // code
                            break;
                        case 0x0E: // LD C,n
                            // code
                            break;
                        case 0x11: // LD DE,nn
                            // code
                            break;
                        case 0x12: // LD (DE),A
                            // code
                            break;
                        case 0x16: // LD D,n
                            // code
                            break;
                        case 0x1A: // LD A,(DE)
                            // code
                            break;
                        case 0x1E: // LD E,n
                            // code
                            break;
                        case 0x21: // LD HL,nn
                            // code
                            break;
                        case 0x22: // LD (nn),HL
                            // code
                            break;
                        case 0x26: // LD H,n
                            // code
                            break;
                        case 0x2A: // LD HL,(nn)
                            // code
                            break;
                        case 0x2E: // LD L,n
                            // code
                            break;
                        case 0x31: // LD SP,nn
                            // code
                            break;
                        case 0x32: // LD (nn),A
                            // code
                            break;
                        case 0x36: // LD (HL),n
                            // code
                            break;
                        case 0x3A: // LD A,(nn)
                            // code
                            break;
                        case 0x3E: // LD A,n
                            // code
                            break;
                        case 0x40: // LD B,B
                            // code
                            break;
                        case 0x41: // LD B,C
                            // code
                            break;
                        case 0x42: // LD B,D
                            // code
                            break;
                        case 0x43: // LD B,E
                            // code
                            break;
                        case 0x44: // LD B,H
                            // code
                            break;
                        case 0x45: // LD B,L
                            // code
                            break;
                        case 0x47: // LD B,A
                            // code
                            break;
                        case 0x46: // LD B,(HL)
                            // code
                            break;
                        case 0x48: // LD C,B
                            // code
                            break;
                        case 0x49: // LD C,C
                            // code
                            break;
                        case 0x4A: // LD C,D
                            // code
                            break;
                        case 0x4B: // LD C,E
                            // code
                            break;
                        case 0x4C: // LD C,H
                            // code
                            break;
                        case 0x4D: // LD C,L
                            // code
                            break;
                        case 0x4F: // LD C,A
                            // code
                            break;
                        case 0x4E: // LD C,(HL)
                            // code
                            break;
                        case 0x50: // LD D,B
                            // code
                            break;
                        case 0x51: // LD D,C
                            // code
                            break;
                        case 0x52: // LD D,D
                            // code
                            break;
                        case 0x53: // LD D,E
                            // code
                            break;
                        case 0x54: // LD D,H
                            // code
                            break;
                        case 0x55: // LD D,L
                            // code
                            break;
                        case 0x57: // LD D,A
                            // code
                            break;
                        case 0x56: // LD D,(HL)
                            // code
                            break;
                        case 0x58: // LD E,B
                            // code
                            break;
                        case 0x59: // LD E,C
                            // code
                            break;
                        case 0x5A: // LD E,D
                            // code
                            break;
                        case 0x5B: // LD E,E
                            // code
                            break;
                        case 0x5C: // LD E,H
                            // code
                            break;
                        case 0x5D: // LD E,L
                            // code
                            break;
                        case 0x5F: // LD E,A
                            // code
                            break;
                        case 0x5E: // LD E,(HL)
                            // code
                            break;
                        case 0x60: // LD H,B
                            // code
                            break;
                        case 0x61: // LD H,C
                            // code
                            break;
                        case 0x62: // LD H,D
                            // code
                            break;
                        case 0x63: // LD H,E
                            // code
                            break;
                        case 0x64: // LD H,H
                            // code
                            break;
                        case 0x65: // LD H,L
                            // code
                            break;
                        case 0x67: // LD H,A
                            // code
                            break;
                        case 0x66: // LD H,(HL)
                            // code
                            break;
                        case 0x68: // LD L,B
                            // code
                            break;
                        case 0x69: // LD L,C
                            // code
                            break;
                        case 0x6A: // LD L,D
                            // code
                            break;
                        case 0x6B: // LD L,E
                            // code
                            break;
                        case 0x6C: // LD L,H
                            // code
                            break;
                        case 0x6D: // LD L,L
                            // code
                            break;
                        case 0x6F: // LD L,A
                            // code
                            break;
                        case 0x6E: // LD L,(HL)
                            // code
                            break;
                        case 0x70: // LD (HL),B
                            // code
                            break;
                        case 0x71: // LD (HL),C
                            // code
                            break;
                        case 0x72: // LD (HL),D
                            // code
                            break;
                        case 0x73: // LD (HL),E
                            // code
                            break;
                        case 0x74: // LD (HL),H
                            // code
                            break;
                        case 0x75: // LD (HL),L
                            // code
                            break;
                        case 0x77: // LD (HL),A
                            // code
                            break;
                        case 0x78: // LD A,B
                            // code
                            break;
                        case 0x79: // LD A,C
                            // code
                            break;
                        case 0x7A: // LD A,D
                            // code
                            break;
                        case 0x7B: // LD A,E
                            // code
                            break;
                        case 0x7C: // LD A,H
                            // code
                            break;
                        case 0x7D: // LD A,L
                            // code
                            break;
                        case 0x7F: // LD A,A
                            // code
                            break;
                        case 0x7E: // LD A,(HL)
                            // code
                            break;
                        case 0xF9: // LD SP,HL
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.CB:
                    switch (instruction.Opcode)
                    {
                        case 0x01: // LD BC,nn
                            // code
                            break;
                        case 0x02: // LD (BC),A
                            // code
                            break;
                        case 0x06: // LD B,n
                            // code
                            break;
                        case 0x0A: // LD A,(BC)
                            // code
                            break;
                        case 0x0E: // LD C,n
                            // code
                            break;
                        case 0x11: // LD DE,nn
                            // code
                            break;
                        case 0x12: // LD (DE),A
                            // code
                            break;
                        case 0x16: // LD D,n
                            // code
                            break;
                        case 0x1A: // LD A,(DE)
                            // code
                            break;
                        case 0x1E: // LD E,n
                            // code
                            break;
                        case 0x21: // LD HL,nn
                            // code
                            break;
                        case 0x22: // LD (nn),HL
                            // code
                            break;
                        case 0x26: // LD H,n
                            // code
                            break;
                        case 0x2A: // LD HL,(nn)
                            // code
                            break;
                        case 0x2E: // LD L,n
                            // code
                            break;
                        case 0x31: // LD SP,nn
                            // code
                            break;
                        case 0x32: // LD (nn),A
                            // code
                            break;
                        case 0x36: // LD (HL),n
                            // code
                            break;
                        case 0x3A: // LD A,(nn)
                            // code
                            break;
                        case 0x3E: // LD A,n
                            // code
                            break;
                        case 0x40: // LD B,B
                            // code
                            break;
                        case 0x41: // LD B,C
                            // code
                            break;
                        case 0x42: // LD B,D
                            // code
                            break;
                        case 0x43: // LD B,E
                            // code
                            break;
                        case 0x44: // LD B,H
                            // code
                            break;
                        case 0x45: // LD B,L
                            // code
                            break;
                        case 0x47: // LD B,A
                            // code
                            break;
                        case 0x46: // LD B,(HL)
                            // code
                            break;
                        case 0x48: // LD C,B
                            // code
                            break;
                        case 0x49: // LD C,C
                            // code
                            break;
                        case 0x4A: // LD C,D
                            // code
                            break;
                        case 0x4B: // LD C,E
                            // code
                            break;
                        case 0x4C: // LD C,H
                            // code
                            break;
                        case 0x4D: // LD C,L
                            // code
                            break;
                        case 0x4F: // LD C,A
                            // code
                            break;
                        case 0x4E: // LD C,(HL)
                            // code
                            break;
                        case 0x50: // LD D,B
                            // code
                            break;
                        case 0x51: // LD D,C
                            // code
                            break;
                        case 0x52: // LD D,D
                            // code
                            break;
                        case 0x53: // LD D,E
                            // code
                            break;
                        case 0x54: // LD D,H
                            // code
                            break;
                        case 0x55: // LD D,L
                            // code
                            break;
                        case 0x57: // LD D,A
                            // code
                            break;
                        case 0x56: // LD D,(HL)
                            // code
                            break;
                        case 0x58: // LD E,B
                            // code
                            break;
                        case 0x59: // LD E,C
                            // code
                            break;
                        case 0x5A: // LD E,D
                            // code
                            break;
                        case 0x5B: // LD E,E
                            // code
                            break;
                        case 0x5C: // LD E,H
                            // code
                            break;
                        case 0x5D: // LD E,L
                            // code
                            break;
                        case 0x5F: // LD E,A
                            // code
                            break;
                        case 0x5E: // LD E,(HL)
                            // code
                            break;
                        case 0x60: // LD H,B
                            // code
                            break;
                        case 0x61: // LD H,C
                            // code
                            break;
                        case 0x62: // LD H,D
                            // code
                            break;
                        case 0x63: // LD H,E
                            // code
                            break;
                        case 0x64: // LD H,H
                            // code
                            break;
                        case 0x65: // LD H,L
                            // code
                            break;
                        case 0x67: // LD H,A
                            // code
                            break;
                        case 0x66: // LD H,(HL)
                            // code
                            break;
                        case 0x68: // LD L,B
                            // code
                            break;
                        case 0x69: // LD L,C
                            // code
                            break;
                        case 0x6A: // LD L,D
                            // code
                            break;
                        case 0x6B: // LD L,E
                            // code
                            break;
                        case 0x6C: // LD L,H
                            // code
                            break;
                        case 0x6D: // LD L,L
                            // code
                            break;
                        case 0x6F: // LD L,A
                            // code
                            break;
                        case 0x6E: // LD L,(HL)
                            // code
                            break;
                        case 0x70: // LD (HL),B
                            // code
                            break;
                        case 0x71: // LD (HL),C
                            // code
                            break;
                        case 0x72: // LD (HL),D
                            // code
                            break;
                        case 0x73: // LD (HL),E
                            // code
                            break;
                        case 0x74: // LD (HL),H
                            // code
                            break;
                        case 0x75: // LD (HL),L
                            // code
                            break;
                        case 0x77: // LD (HL),A
                            // code
                            break;
                        case 0x78: // LD A,B
                            // code
                            break;
                        case 0x79: // LD A,C
                            // code
                            break;
                        case 0x7A: // LD A,D
                            // code
                            break;
                        case 0x7B: // LD A,E
                            // code
                            break;
                        case 0x7C: // LD A,H
                            // code
                            break;
                        case 0x7D: // LD A,L
                            // code
                            break;
                        case 0x7F: // LD A,A
                            // code
                            break;
                        case 0x7E: // LD A,(HL)
                            // code
                            break;
                        case 0xF9: // LD SP,HL
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.ED:
                    switch (instruction.Opcode)
                    {
                        case 0x43: // LD (nn),BC
                            // code
                            break;
                        case 0x47: // LD I,A
                            // code
                            break;
                        case 0x4B: // LD BC,(nn)
                            // code
                            break;
                        case 0x4F: // LD R,A
                            // code
                            break;
                        case 0x53: // LD (nn),DE
                            // code
                            break;
                        case 0x57: // LD A,I
                            // code
                            break;
                        case 0x5B: // LD DE,(nn)
                            // code
                            break;
                        case 0x5F: // LD A,R
                            // code
                            break;
                        case 0x73: // LD (nn),SP
                            // code
                            break;
                        case 0x7B: // LD SP,(nn)
                            // code
                            break;
                        case 0xA0: // LDI
                            // code
                            break;
                        case 0xA8: // LDD
                            // code
                            break;
                        case 0xB0: // LDIR
                            // code
                            break;
                        case 0xB8: // LDDR
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.DD:
                    switch (instruction.Opcode)
                    {
                        case 0x21: // LD IX,nn
                            // code
                            break;
                        case 0x22: // LD (nn),IX
                            // code
                            break;
                        case 0x26: // LD IXh,n
                            // code
                            break;
                        case 0x2A: // LD IX,(nn)
                            // code
                            break;
                        case 0x2E: // LD IXl,n
                            // code
                            break;
                        case 0x36: // LD (IX+o),n
                            // code
                            break;
                        case 0x44: // LD B,IXh
                            // code
                            break;
                        case 0x45: // LD B,IXl
                            // code
                            break;
                        case 0x46: // LD B,(IX+o)
                            // code
                            break;
                        case 0x4C: // LD C,IXh
                            // code
                            break;
                        case 0x4D: // LD C,IXl
                            // code
                            break;
                        case 0x4E: // LD C,(IX+o)
                            // code
                            break;
                        case 0x54: // LD D,IXh
                            // code
                            break;
                        case 0x55: // LD D,IXl
                            // code
                            break;
                        case 0x56: // LD D,(IX+o)
                            // code
                            break;
                        case 0x5C: // LD E,IXh
                            // code
                            break;
                        case 0x5D: // LD E,IXl
                            // code
                            break;
                        case 0x5E: // LD E,(IX+o)
                            // code
                            break;
                        case 0x60: // LD IXh,B
                            // code
                            break;
                        case 0x61: // LD IXh,C
                            // code
                            break;
                        case 0x62: // LD IXh,D
                            // code
                            break;
                        case 0x63: // LD IXh,E
                            // code
                            break;
                        case 0x64: // LD IXh,IXh
                            // code
                            break;
                        case 0x65: // LD IXh,IXl
                            // code
                            break;
                        case 0x67: // LD IXh,A
                            // code
                            break;
                        case 0x66: // LD H,(IX+o)
                            // code
                            break;
                        case 0x68: // LD IXl,B
                            // code
                            break;
                        case 0x69: // LD IXl,C
                            // code
                            break;
                        case 0x6A: // LD IXl,D
                            // code
                            break;
                        case 0x6B: // LD IXl,E
                            // code
                            break;
                        case 0x6C: // LD IXl,IXh
                            // code
                            break;
                        case 0x6D: // LD IXl,IXl
                            // code
                            break;
                        case 0x6F: // LD IXl,A
                            // code
                            break;
                        case 0x6E: // LD L,(IX+o)
                            // code
                            break;
                        case 0x70: // LD (IX+o),B
                            // code
                            break;
                        case 0x71: // LD (IX+o),C
                            // code
                            break;
                        case 0x72: // LD (IX+o),D
                            // code
                            break;
                        case 0x73: // LD (IX+o),E
                            // code
                            break;
                        case 0x74: // LD (IX+o),H
                            // code
                            break;
                        case 0x75: // LD (IX+o),L
                            // code
                            break;
                        case 0x77: // LD (IX+o),A
                            // code
                            break;
                        case 0x7C: // LD A,IXh
                            // code
                            break;
                        case 0x7D: // LD A,IXl
                            // code
                            break;
                        case 0x7E: // LD A,(IX+o)
                            // code
                            break;
                        case 0xF9: // LD SP,IX
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.FD:
                    switch (instruction.Opcode)
                    {
                        case 0x21: // LD IY,nn
                            // code
                            break;
                        case 0x22: // LD (nn),IY
                            // code
                            break;
                        case 0x26: // LD IYh,n
                            // code
                            break;
                        case 0x2A: // LD IY,(nn)
                            // code
                            break;
                        case 0x2E: // LD IYl,n
                            // code
                            break;
                        case 0x36: // LD (IY+o),n
                            // code
                            break;
                        case 0x44: // LD B,IYh
                            // code
                            break;
                        case 0x45: // LD B,IYl
                            // code
                            break;
                        case 0x46: // LD B,(IY+o)
                            // code
                            break;
                        case 0x4C: // LD C,IYh
                            // code
                            break;
                        case 0x4D: // LD C,IYl
                            // code
                            break;
                        case 0x4E: // LD C,(IY+o)
                            // code
                            break;
                        case 0x54: // LD D,IYh
                            // code
                            break;
                        case 0x55: // LD D,IYl
                            // code
                            break;
                        case 0x56: // LD D,(IY+o)
                            // code
                            break;
                        case 0x5C: // LD E,IYh
                            // code
                            break;
                        case 0x5D: // LD E,IYl
                            // code
                            break;
                        case 0x5E: // LD E,(IY+o)
                            // code
                            break;
                        case 0x60: // LD IYh,B
                            // code
                            break;
                        case 0x61: // LD IYh,C
                            // code
                            break;
                        case 0x62: // LD IYh,D
                            // code
                            break;
                        case 0x63: // LD IYh,E
                            // code
                            break;
                        case 0x64: // LD IYh,IYh
                            // code
                            break;
                        case 0x65: // LD IYh,IYl
                            // code
                            break;
                        case 0x67: // LD IYh,A
                            // code
                            break;
                        case 0x66: // LD H,(IY+o)
                            // code
                            break;
                        case 0x68: // LD IYl,B
                            // code
                            break;
                        case 0x69: // LD IYl,C
                            // code
                            break;
                        case 0x6A: // LD IYl,D
                            // code
                            break;
                        case 0x6B: // LD IYl,E
                            // code
                            break;
                        case 0x6C: // LD IYl,IYh
                            // code
                            break;
                        case 0x6D: // LD IYl,IYl
                            // code
                            break;
                        case 0x6F: // LD IYl,A
                            // code
                            break;
                        case 0x6E: // LD L,(IY+o)
                            // code
                            break;
                        case 0x70: // LD (IY+o),B
                            // code
                            break;
                        case 0x71: // LD (IY+o),C
                            // code
                            break;
                        case 0x72: // LD (IY+o),D
                            // code
                            break;
                        case 0x73: // LD (IY+o),E
                            // code
                            break;
                        case 0x74: // LD (IY+o),H
                            // code
                            break;
                        case 0x75: // LD (IY+o),L
                            // code
                            break;
                        case 0x77: // LD (IY+o),A
                            // code
                            break;
                        case 0x7C: // LD A,IYh
                            // code
                            break;
                        case 0x7D: // LD A,IYl
                            // code
                            break;
                        case 0x7E: // LD A,(IY+o)
                            // code
                            break;
                        case 0xF9: // LD SP,IY
                            // code
                            break;

                    }
                    break;

                case InstructionPrefix.DDCB:
                    switch (instruction.Opcode)
                    {

                    }
                    break;

                case InstructionPrefix.FDCB:
                    switch (instruction.Opcode)
                    {

                    }
                    break;
            }

            return new ExecutionResult(new Flags(), 0);
        }
    }
}

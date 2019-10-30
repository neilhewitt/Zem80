using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z80.Core
{
    public partial class InstructionDecoder
    {
        public InstructionPackage Decode(byte[] instructionBytes, ushort address)
        {
            byte opcode = 0x00;
            InstructionData data = new InstructionData();

            try
            {
                // handle NOP + special case - sequences of DD or FD (uniform or mixed) count as NOP until the final DD / FD
                if (instructionBytes[0] == 0x00 ||
                    (instructionBytes[0] == 0xDD || instructionBytes[0] == 0xFD) && (instructionBytes[1] == 0xDD || instructionBytes[1] == 0xFD))
                {
                    return new InstructionPackage(Instruction.NOP, data);
                }

                byte prefix = instructionBytes[0];
                data.IndexIX = prefix == 0xDD;
                data.IndexIY = prefix == 0xFD;

                if (prefix == 0xCB || ((prefix == 0xDD || prefix == 0xFD) && instructionBytes[1] == 0xCB))
                {
                    // extended instructions (including double-prefix 'DD CB' and 'FD CD')
                    bool doubleBytePrefixed = prefix != 0xCB;
                    opcode = doubleBytePrefixed ? instructionBytes[3] : instructionBytes[1];

                    Instruction instruction = doubleBytePrefixed ?
                        Instruction.Find(opcode, prefix == 0xDD ? InstructionPrefix.DDCB : InstructionPrefix.FDCB) :
                        Instruction.Find(opcode, InstructionPrefix.CB);

                    if (instruction.Modifier == ModifierType.Register) // +r
                    {
                        data.RegisterIndex = GetRegisterIndex(opcode);
                    }

                    if (instruction.Modifier == ModifierType.Bit) // +8*b
                    {
                        data.BitIndex = GetBitIndex(opcode);
                    }

                    if (instruction.Argument1 == ArgumentType.Displacement)
                    {
                        data.Argument1 = instructionBytes[2];
                    }

                    data.Opcode = opcode;
                    return new InstructionPackage(instruction, data);
                }
                else if (prefix == 0xED)
                {
                    // other extended instructions
                    opcode = instructionBytes[1];
                    Instruction instruction = Instruction.Find(opcode, InstructionPrefix.ED);

                    if (instruction.Argument1 == ArgumentType.ImmediateWord)
                    {
                        data.Argument1 = instructionBytes[2];
                        data.Argument2 = instructionBytes[3];
                    }

                    data.Opcode = opcode;
                    return new InstructionPackage(instruction, data);
                }
                else if (prefix == 0xDD || prefix == 0xFD)
                {
                    opcode = instructionBytes[1];
                    Instruction instruction = Instruction.Find(opcode, prefix == 0xDD ? InstructionPrefix.DD : InstructionPrefix.FD);

                    if (instruction.Modifier == ModifierType.IndexRegister) // +p / +q
                    {
                        data.RegisterIndex = GetRegisterIndex(opcode);
                        data.DirectIX = (prefix == 0xDD && (data.RegisterIndex == RegisterIndex.H || data.RegisterIndex == RegisterIndex.L)); // IX substituted for HL
                        data.DirectIY = (prefix == 0xFD && (data.RegisterIndex == RegisterIndex.H || data.RegisterIndex == RegisterIndex.L)); // IY substituted for HL
                    }

                    if (instruction.Modifier == ModifierType.IndexRegisterHalf) // +8*p / +8*q
                    {
                        data.RegisterIndex = GetRegisterIndex(opcode); // will be either H (representing IXh) or L (representing IXl)
                    }

                    if (instruction.Argument1 == ArgumentType.Displacement || instruction.Argument1 == ArgumentType.Immediate)
                    {
                        data.Argument1 = instructionBytes[2];
                    }
                    else if (instruction.Argument1 == ArgumentType.ImmediateWord)
                    {
                        data.Argument1 = instructionBytes[2];
                        data.Argument2 = instructionBytes[3];
                    }

                    data.Opcode = opcode;
                    return new InstructionPackage(instruction, data);

                }
                else
                {
                    prefix = 0x00;
                    opcode = instructionBytes[0];

                    Instruction instruction = Instruction.Find(opcode, InstructionPrefix.Unprefixed);

                    if (instruction.Modifier == ModifierType.Register) // +r
                    {
                        data.RegisterIndex = GetRegisterIndex(opcode);
                    }

                    if (instruction.Argument1 == ArgumentType.Displacement || instruction.Argument1 == ArgumentType.Immediate)
                    {
                        data.Argument1 = instructionBytes[1];
                    }
                    else if (instruction.Argument1 == ArgumentType.ImmediateWord)
                    {
                        data.Argument1 = instructionBytes[1];
                        data.Argument2 = instructionBytes[2];
                    }

                    data.Opcode = opcode;
                    return new InstructionPackage(instruction, data);
                }
            }
            catch (IndexOutOfRangeException)
            {
                // handle special case where instruction buffer is short / corrupt (eg read beyond end of memory) in which case, return null to the caller.
                // all other exceptions will throw as normal.
                return null;
            }
        }

        private RegisterIndex GetRegisterIndex(byte opcode)
        {
            return (RegisterIndex)opcode.RemoveBits(3, 5); // register is first 3 bits
        }

        private byte GetBitIndex(byte opcode)
        {
            return opcode.GetByteFromBits(3, 3); // bitindex is bits 3-5
        }

        public InstructionDecoder()
        { }
    }
}

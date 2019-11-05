﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z80.Core
{
    public class InstructionDecoder
    {
        public InstructionPackage Decode(byte[] instructionBytes)
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
                    // identical to unprefixed instruction with same opcode except IX (0xDD) or IY (0xFD) replaces HL
                    // plus additional instructions unique to IX / IY including single-byte operations on high / low bytes of either

                    opcode = instructionBytes[1];
                    Instruction instruction = Instruction.Find(opcode, prefix == 0xDD ? InstructionPrefix.DD : InstructionPrefix.FD);

                    if (instruction.Modifier == ModifierType.IndexRegister) // +p / +q
                    {
                        data.RegisterIndex = GetRegisterIndex(opcode);
                        data.DirectIX = (prefix == 0xDD && (data.RegisterIndex == RegisterIndex.IXh || data.RegisterIndex == RegisterIndex.IXl)); // IX substituted for HL
                        data.DirectIY = (prefix == 0xFD && (data.RegisterIndex == RegisterIndex.IYh || data.RegisterIndex == RegisterIndex.IYl)); // IY substituted for HL
                    }

                    if (instruction.Modifier == ModifierType.IndexRegisterHalf) // +8*p / +8*q
                    {
                        data.RegisterIndex = GetRegisterIndex(opcode); 
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
                    // unprefixed instructions (can be 1-3 bytes only)

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
                // handle special case where instruction buffer is short / corrupt (read beyond end of memory) in which case, return null to the caller
                // all other exceptions will throw as normal
                return null;
            }
        }

        public InstructionPackage DecodeInterrupt(Func<byte> dataRead)
        {
            if (dataRead == null) throw new Z80Exception("You must supply a valid delegate / Func<bool> to support reading device data.");

            byte[] instructionBytes = new byte[4];
            instructionBytes[0] = dataRead();

            if (instructionBytes[0] == 0xCB || instructionBytes[0] == 0xDD || instructionBytes[0] == 0xED || instructionBytes[0] == 0xFD)
            {
                // we have a prefix... need the next byte
                instructionBytes[1] = dataRead();

                if (instructionBytes[0] == 0xCB && (instructionBytes[1] == 0xDD || instructionBytes[1] == 0xFD))
                {
                    // double-byte prefix - read next 2 bytes from device and decode (always 4 bytes)
                    instructionBytes[2] = dataRead();
                    instructionBytes[3] = dataRead();
                }
                else
                {
                    byte opcode = instructionBytes[1];
                    Instruction instruction = Instruction.Find(instructionBytes[0], (InstructionPrefix)instructionBytes[1]);
                    for (int i = 2; i <= instruction.SizeInBytes; i++) // 2 - 4 bytes
                    {
                        instructionBytes[i] = dataRead();
                    }
                }
            }
            else
            {
                byte opcode = instructionBytes[0];
                Instruction instruction = Instruction.Find(opcode, InstructionPrefix.Unprefixed);
                for (int i = 1; i <= instruction.SizeInBytes; i++) // 1 - 3 bytes
                {
                    instructionBytes[i] = dataRead();
                }
            }

            return Decode(instructionBytes);
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
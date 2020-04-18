using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z80.Core
{
    public class InstructionDecoder
    {
        public ExecutionPackage Decode(byte[] instructionBytes)
        {
            byte b0 = instructionBytes[0];
            byte b1 = instructionBytes[1];
            byte b2 = instructionBytes[2];
            byte b3 = instructionBytes[3];
            InstructionData data = new InstructionData();

            try
            {
                // handle NOP + special case - sequences of DD or FD (uniform or mixed) count as NOP until the final DD / FD
                if (b0 == 0x00 ||
                    (b0 == 0xDD || b0 == 0xFD) && (b1 == 0xDD || b1 == 0xFD))
                {
                    return new ExecutionPackage(Instruction.NOP, data);
                }

                byte prefix = b0;

                if (prefix == 0xCB || ((prefix == 0xDD || prefix == 0xFD) && b1 == 0xCB))
                {
                    // extended instructions (including double-prefix 'DD CB' and 'FD CB')
                    Instruction instruction = Instruction.Find((prefix != 0xCB) ? b3 : b1, (InstructionPrefix)prefix);

                    if (instruction.Argument1 == ArgumentType.Displacement)
                    {
                        data.Argument1 = b2;
                    }

                    return new ExecutionPackage(instruction, data);
                }
                else if (prefix == 0xED)
                {
                    // other extended instructions
                    Instruction instruction = Instruction.Find(b1, InstructionPrefix.ED);

                    if (instruction.Argument1 == ArgumentType.ImmediateWord)
                    {
                        data.Argument1 = b2;
                        data.Argument2 = b3;
                    }

                    return new ExecutionPackage(instruction, data);
                }
                else if (prefix == 0xDD || prefix == 0xFD)
                {
                    // identical to unprefixed instruction with same opcode except IX (0xDD) or IY (0xFD) replaces HL
                    // plus additional instructions unique to IX / IY including single-byte operations on high / low bytes of either

                    Instruction instruction = Instruction.Find(b1, (InstructionPrefix)prefix);

                    if (instruction.Argument1 == ArgumentType.Displacement || instruction.Argument1 == ArgumentType.Immediate)
                    {
                        data.Argument1 = b2;
                    }
                    else if (instruction.Argument1 == ArgumentType.ImmediateWord)
                    {
                        data.Argument1 = b2;
                        data.Argument2 = b3;
                    }

                    return new ExecutionPackage(instruction, data);

                }
                else
                {
                    // unprefixed instructions (can be 1-3 bytes only)
                    Instruction instruction = Instruction.Find(b0, InstructionPrefix.Unprefixed);

                    if (instruction.Argument1 == ArgumentType.Displacement || instruction.Argument1 == ArgumentType.Immediate)
                    {
                        data.Argument1 = b1;
                    }
                    else if (instruction.Argument1 == ArgumentType.ImmediateWord)
                    {
                        data.Argument1 = b1;
                        data.Argument2 = b2;
                    }

                    return new ExecutionPackage(instruction, data);
                }
            }
            catch (InstructionNotFoundException)
            {
                // handle special case where instruction buffer is short / corrupt (read beyond end of memory) in which case, return null to the caller
                // all other exceptions will throw as normal
                return null;
            }
        }

        public ExecutionPackage DecodeInterrupt(Func<byte> dataRead)
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
                    // could be any size instruction between 2 and 4 bytes
                    // find instruction and check size, then assemble instruction bytes from device
                    Instruction instruction = Instruction.Find(instructionBytes[0], (InstructionPrefix)instructionBytes[1]);
                    for (int i = 2; i <= instruction.SizeInBytes; i++) // 2 - 4 bytes
                    {
                        instructionBytes[i] = dataRead();
                    }
                }
            }
            else
            {
                // could be any size instruction between 1 and 3 bytes
                // find instruction and check size, then assemble instruction bytes from device
                Instruction instruction = Instruction.Find(instructionBytes[0], InstructionPrefix.Unprefixed);
                for (int i = 1; i <= instruction.SizeInBytes; i++) // 1 - 3 bytes
                {
                    instructionBytes[i] = dataRead();
                }
            }

            return Decode(instructionBytes);
        }

        public InstructionDecoder()
        { }
    }
}

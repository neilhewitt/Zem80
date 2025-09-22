using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Zem80.Core.CPU
{
    public static class InstructionDecoder
    {
        public static InstructionPackage DecodeInstruction(byte[] instructionBytes, ushort address, out bool skipNextByte, out bool opcodeErrorNOP)
        {
            if (instructionBytes.Length != 4) throw new InstructionDecoderException("Supplied byte array must be 4 bytes long.");

            byte b0, b1, b2, b3;
            Instruction instruction;
            InstructionData data = new InstructionData();

            skipNextByte = false;
            opcodeErrorNOP = false;

            b0 = instructionBytes[0];
            b1 = instructionBytes[1];
            b2 = instructionBytes[2];
            b3 = instructionBytes[3];

            // was byte 0 a prefix code?
            if (b0 == 0xCB || b0 == 0xDD || b0 == 0xED || b0 == 0xFD)
            {
                if ((b0 == 0xDD || b0 == 0xFD || b0 == 0xED) && (b1 == 0xDD || b1 == 0xFD || b1 == 0xED))
                {
                    // sequences of 0xDD / 0xFD / 0xED count as NOP until the final 0xDD / 0xFD / 0xED which is then the prefix byte
                    // for this pair of bytes, we will decode a NOP and move the program counter on
                    instruction = InstructionSet.NOP; // this *doesn't* count as an opcode error even though it's a 'synthetic' NOP
                }
                else if ((b0 == 0xDD || b0 == 0xFD) && b1 == 0xCB)
                {
                    // DDCB / FDCB: four-byte opcode = two prefix bytes + one displacement byte + one opcode byte (no four-byte instruction has any actual operand values)
                    if (!InstructionSet.Instructions.TryGetValue(b3 | b1 << 8 | b0 << 16, out instruction))
                    {
                        // not a valid instruction - the Z80 spec says we should run a single NOP instead
                        instruction = InstructionSet.NOP;
                        opcodeErrorNOP = true;
                    }
                    else
                    {
                        data.Argument1 = b2; // byte 2 is the displacement value, which goes into Argument1
                    }
                }
                else
                {
                    // all other prefixed instructions (CB, ED, DD, FD): a two-byte opcode + up to 2 operand bytes
                    if (!InstructionSet.Instructions.TryGetValue(b1 | b0 << 8, out instruction))
                    {
                        // if prefix was 0xED and the instruction is invalid, the spec says run *two* NOPs
                        if (b0 == 0xED)
                        {
                            skipNextByte = true; // causes the processor to run an extra NOP *after* this one and skip over the invalid instruction byte
                        }

                        // otherwise, if the prefix was 0xDD or 0xFD and the instruction is invalid, the spec says we should run a NOP now but then run the equivalent
                        // unprefixed instruction next - this will happen automatically when PC advances past the synthetic NOP
                        instruction = InstructionSet.NOP;
                        opcodeErrorNOP = true;
                    }
                    else
                    {
                        // decode arguments and set the values accordingly
                        if (instruction.Argument1 != InstructionElement.None)
                        {
                            data.Argument1 = b2;
                            if (instruction.Argument2 != InstructionElement.None)
                            {
                                data.Argument2 = b3;
                            }
                        }
                    }
                }
            }
            else
            {
                // unprefixed instruction - 1 byte opcode + up to 2 operand bytes
                if (!InstructionSet.Instructions.TryGetValue(b0, out instruction))
                {
                    instruction = InstructionSet.NOP;
                }     
                else
                {
                    if (instruction.Argument1 != InstructionElement.None)
                    {
                        data.Argument1 = b1;
                        if (instruction.Argument2 != InstructionElement.None)
                        {
                            data.Argument2 = b2;
                        }
                    }
                }
            }

            return new InstructionPackage(instruction, data, address);
        }
    }
}

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
                if (b0 == 0x00 || ((b0 == 0xDD || b0 == 0xFD) && (b1 == 0xDD || b1 == 0xFD)))
                {
                    return new ExecutionPackage(InstructionSet.NOP, data);
                }

                // b0 may be a prefix byte (or the first byte of two prefix bytes), and can be any of:
                // CB, ED, DD, FD, DD+CB or DD+FD

                byte prefix = b0;
                Instruction instruction = null;

                if (((prefix == 0xDD || prefix == 0xFD) && b1 == 0xCB))
                {
                    // extended instructions (double-prefix 'DD CB' and 'FD CB')
                    // prefix is always followed by a displacement byte or 0x00, opcode is then in *b3*
                    instruction = InstructionSet.Find(b3, (InstructionPrefix)(prefix, b1).ToWord());

                    // only displacement arguments valid for DDCB/FDCB instructions
                    if (instruction.Argument1 == ArgumentType.Displacement)
                    {
                        data.Argument1 = b2;
                    }

                    return new ExecutionPackage(instruction, data);
                }
                else if (prefix == 0xCB || prefix == 0xED || prefix == 0xDD || prefix == 0xFD)
                {
                    // other extended instructions
                    instruction = InstructionSet.Find(b1, (InstructionPrefix)prefix);
                    if (instruction == null)
                    {
                        if (prefix == 0xED)
                        {
                            byte[] undocumentedED = new byte[] { 0x4C, 0x4E, 0x54, 0x55, 0x5C, 0x5D, 0x64, 0x65, 0x6C, 0x6D, 0x6E, 0x71, 0x74, 0x75, 0x76, 0x77, 0x7C, 0x7D, 0x7E };
                            // some ED instructions are undocumented but are the equivalent of the unprefixed instruction without the prefix
                            // in which case we should behave as per the unprefixed instruction,
                            // but if not on the list above, any missing ED instruction should be treated as 2 x NOP
                            // in this case we return a package with the instruction set to null and the CPU will sort it out
                            if (!undocumentedED.Contains(b1))
                            {
                                return new ExecutionPackage(null, data);
                            }
                        }

                        // ignore the prefix and use the unprefixed instruction, there are holes in the extended instruction set
                        // and some (naughty!) code uses this technique to consume extra cycles for synchronisation
                        return Decode(new byte[4] { b1, b2, b3, 0x00 });
                    }

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
                    instruction = InstructionSet.Find(b0, InstructionPrefix.Unprefixed);

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
            if (dataRead == null) throw new InterruptException("You must supply a valid delegate / Func<bool> to support reading device data.");

            byte[] instructionBytes = new byte[4];
            for (int i = 0; i < 4; i++) instructionBytes[i] = dataRead();

            return Decode(instructionBytes);
        }

        public InstructionDecoder()
        { }
    }
}

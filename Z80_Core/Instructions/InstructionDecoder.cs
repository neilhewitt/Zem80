using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z80.Core
{
    public partial class InstructionDecoder
    {
        public Instruction Decode(MemoryReader memory, ushort address, out InstructionData data)
        {
            byte opcode = 0x00;
            byte[] bytes = memory.ReadBytesAt(address, 4); // read next 4 bytes (max size is 4-byte instruction opcode)

            // special case - sequences of DD or FD count as NOP until the final DD / FD
            while ((bytes[0] == 0xDD && bytes[1] == 0xDD) || (bytes[0] == 0xFD && bytes[1] == 0xFD))
            {
                address++;
                bytes = memory.ReadBytesAt(address, 4);
            }

            byte prefix = bytes[0];

            data = new InstructionData();
            data.IndexIX = prefix == 0xDD;
            data.IndexIY = prefix == 0xFD;

            if (prefix == 0xCB || ((prefix == 0xDD || prefix == 0xFD) && bytes[1] == 0xCB))
            {
                // extended instructions (including double-prefix 'DD CB' and 'FD CD')
                bool extendedIndexed = bytes[1] == 0xCB;
                opcode = extendedIndexed ? bytes[3] : bytes[1];

                InstructionInfo info = extendedIndexed ? 
                    InstructionInfo.For(opcode, bytes[0], bytes[1]) : 
                    InstructionInfo.For(opcode, bytes[0]);

                Instruction instruction = Instruction.Build(opcode, info);

                if (info.Modifier.Contains("+r"))
                {
                    data.RegisterIndex = GetRegisterIndex(opcode);
                }

                if (info.Modifier.Contains("+b"))
                {
                    data.BitIndex = GetBitIndex(opcode);
                }

                if (info.Argument1 == "o")
                {
                    data.Displacement = bytes[2];
                }

                return instruction;
            }
            else if (prefix == 0xED)
            {
                // other extended instructions
                opcode = bytes[1];
                InstructionInfo info = InstructionInfo.For(opcode, prefix);
                Instruction instruction = Instruction.Build(opcode, info);

                if (info.Argument1 == "nn")
                {
                    data.Arguments = new byte[2] { bytes[2], bytes[3] };
                }
                
                return instruction;
            }
            else if (prefix == 0xDD || prefix == 0xFD)
            {
                opcode = bytes[1];
                InstructionInfo info = InstructionInfo.For(opcode, prefix);
                Instruction instruction = Instruction.Build(opcode, info);

                if (info.Modifier == "+p" || info.Modifier == "+q")
                {
                    data.RegisterIndex = GetRegisterIndex(opcode);
                    data.DirectIX = (prefix == 0xDD && (data.RegisterIndex == RegisterIndex.IXh || data.RegisterIndex == RegisterIndex.IXl));
                    data.DirectIY = (prefix == 0xFD && (data.RegisterIndex == RegisterIndex.IYh || data.RegisterIndex == RegisterIndex.IYl));
                }

                if (info.Modifier == "+8*p" || info.Modifier == "+8*q")
                {
                    data.RegisterIndex = GetRegisterIndex(opcode);
                }

                if (info.Argument1 == "o" || info.Argument1 == "n")
                {
                    data.Displacement = bytes[2];
                }
                else if (info.Argument1 == "nn")
                {
                    data.Arguments = new byte[2] { bytes[2], bytes[3] };
                }

                return instruction;

            }
            else
            {
                prefix = 0x00;
                {
                    // basic instructions
                    opcode = bytes[0];

                    InstructionInfo info = InstructionInfo.For(opcode, prefix);
                    Instruction instruction = Instruction.Build(opcode, info);

                    if (info.Modifier == "+r")
                    {
                        data.RegisterIndex = GetRegisterIndex(opcode);
                    }

                    if (info.Argument1 == "o")
                    {
                        data.Displacement = bytes[1];
                    }
                    else if (info.Argument1 == "n")
                    {
                        data.Arguments = new byte[1] { bytes[1] };
                    }
                    else if (info.Argument1 == "nn")
                    {
                        data.Arguments = new byte[2] { bytes[1], bytes[2] };
                    }
                    
                    return instruction;
                }
            }
        }

        private RegisterIndex GetRegisterIndex(byte opcode)
        {
            return (RegisterIndex)opcode.RemoveBits(3, 5); // register is first 3 bits
        }

        private uint GetBitIndex(byte opcode)
        {
            return opcode.RemoveBits(0, 3).RemoveBits(6, 2); // bitindex is bits 3-5
        }

        public InstructionDecoder()
        { }
    }
}

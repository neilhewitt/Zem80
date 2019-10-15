using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z80.Core
{
    public partial class InstructionDecoder
    {
        public InstructionInfo Decode(byte[] instruction, ushort address, out InstructionData data)
        {
            byte opcode = 0x00;
            data = new InstructionData();

            // special case - sequences of DD or FD (uniform or mixed) count as NOP until the final DD / FD
            if ((instruction[0] == 0xDD || instruction[0] == 0xFD) && (instruction[1] == 0xDD || instruction[1] == 0xFD))
            {
                return InstructionInfo.NOP;
            }

            byte prefix = instruction[0];

            data.IndexIX = prefix == 0xDD;
            data.IndexIY = prefix == 0xFD;

            if (prefix == 0xCB || ((prefix == 0xDD || prefix == 0xFD) && instruction[1] == 0xCB))
            {
                // extended instructions (including double-prefix 'DD CB' and 'FD CD')
                bool doubleBytePrefixed = instruction[1] == 0xCB;
                opcode = doubleBytePrefixed ? instruction[3] : instruction[1];

                InstructionInfo info = doubleBytePrefixed ?
                    InstructionInfo.Find(opcode, instruction[0], instruction[1]) :
                    InstructionInfo.Find(opcode, instruction[0]);

                if ((info.Modifier & ModifierType.Register) == ModifierType.Register) // +r
                {
                    data.RegisterIndex = GetRegisterIndex(opcode);
                }

                if ((info.Modifier & ModifierType.Bit) == ModifierType.Bit) // +8*b
                {
                    data.BitIndex = GetBitIndex(opcode);
                }

                if (info.Argument1 == ArgumentType.Displacement)
                {
                    data.Displacement = instruction[2];
                }

                return info;
            }
            else if (prefix == 0xED)
            {
                // other extended instructions
                opcode = instruction[1];
                InstructionInfo info = InstructionInfo.Find(opcode, prefix);

                if (info.Argument1 == ArgumentType.ImmediateWord)
                {
                    data.Arguments = new byte[2] { instruction[2], instruction[3] };
                }

                return info;
            }
            else if (prefix == 0xDD || prefix == 0xFD)
            {
                opcode = instruction[1];
                InstructionInfo info = InstructionInfo.Find(opcode, prefix);

                if ((info.Modifier & ModifierType.IndexRegister) == ModifierType.IndexRegister) // +p / +q
                {
                    data.RegisterIndex = GetRegisterIndex(opcode);
                    data.DirectIX = (prefix == 0xDD && (data.RegisterIndex == RegisterIndex.IXh || data.RegisterIndex == RegisterIndex.IXl));
                    data.DirectIY = (prefix == 0xFD && (data.RegisterIndex == RegisterIndex.IYh || data.RegisterIndex == RegisterIndex.IYl));
                }

                if ((info.Modifier & ModifierType.IndexRegisterHigh) == ModifierType.IndexRegisterHigh) // +8*p / +8*q
                {
                    data.RegisterIndex = GetRegisterIndex(opcode);
                }

                if (info.Argument1 == ArgumentType.Displacement || info.Argument1 == ArgumentType.Immediate)
                {
                    data.Displacement = instruction[2];
                }
                else if (info.Argument1 == ArgumentType.ImmediateWord)
                {
                    data.Arguments = new byte[2] { instruction[2], instruction[3] };
                }

                return info;

            }
            else
            {
                prefix = 0x00;
                opcode = instruction[0];

                InstructionInfo info = InstructionInfo.Find(opcode, prefix);

                if ((info.Modifier & ModifierType.Register) == ModifierType.Register) // +r
                {
                    data.RegisterIndex = GetRegisterIndex(opcode);
                }

                if (info.Argument1 == ArgumentType.Displacement)
                {
                    data.Displacement = instruction[1];
                }
                else if (info.Argument1 == ArgumentType.Immediate)
                {
                    data.Arguments = new byte[1] { instruction[1] };
                }
                else if (info.Argument1 == ArgumentType.ImmediateWord)
                {
                    data.Arguments = new byte[2] { instruction[1], instruction[2] };
                }

                return info;
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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Z80.Core
{
    public static class InstructionExtensions
    {
        public static byte GetBitIndex(this Instruction instruction)
        {
            return instruction.Opcode.GetByteFromBits(3, 3);
        }

        public static byte MarshalSourceByte(this Instruction instruction, InstructionData data, Processor cpu, out ushort address, out ByteRegister source)
        {
            Registers r = cpu.Registers;
            address = 0x0000;

            byte value;
            source = instruction.Source.AsByteRegister();
            if (source != ByteRegister.None)
            {
                value = r[source];
            }
            else
            {
                if (instruction.Argument1 == InstructionElement.ByteValue)
                {
                    value = data.Argument1;
                }
                else if (instruction.Source == InstructionElement.None)
                {
                    address = instruction.Target.AsWordRegister() switch
                    {
                        WordRegister.IX => (ushort)(r.IX + (sbyte)data.Argument1),
                        WordRegister.IY => (ushort)(r.IY + (sbyte)data.Argument1),
                        _ => r.HL
                    };

                    value = cpu.Memory.ReadByteAt(address, false);
                }
                else
                {
                    address = instruction.Source.AsWordRegister() switch
                    {
                        WordRegister.IX => (ushort)(r.IX + (sbyte)data.Argument1),
                        WordRegister.IY => (ushort)(r.IY + (sbyte)data.Argument1),
                        _ => r.HL
                    };

                    value = cpu.Memory.ReadByteAt(address, false);
                }
            }

            return value;
        }

        public static ushort MarshalSourceWord(this Instruction instruction, InstructionData data, Processor cpu, out ushort address)
        {
            Registers r = cpu.Registers;
            address = 0x0000;

            ushort value;
            WordRegister source = instruction.Source.AsWordRegister();
            if (source != WordRegister.None)
            {
                value = r[source];
            }
            else
            {
                if (instruction.Argument1 == InstructionElement.ByteValue && instruction.Argument2 == InstructionElement.ByteValue)
                {
                    value = data.ArgumentsAsWord;
                }
                else
                {
                    address = instruction.Source.AsWordRegister() switch
                    {
                        WordRegister.IX => (ushort)(r.IX + (sbyte)data.Argument1),
                        WordRegister.IY => (ushort)(r.IY + (sbyte)data.Argument1),
                        _ => r.HL
                    };

                    value = cpu.Memory.ReadByteAt(address, false);
                }
            }

            return value;
        }
    }
}

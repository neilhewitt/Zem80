using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Zem80.Core.CPU
{
    public static class Resolver
    {
        public static byte GetSourceByte(Instruction instruction, InstructionData data, Processor cpu, byte memoryReadTStates)
        {
            return GetSourceByte(instruction, data, cpu, out ushort _, memoryReadTStates);
        }

        public static byte GetSourceByte(Instruction instruction, InstructionData data, Processor cpu, out ushort address, byte memoryReadTStates)
        {
            // this fetches a byte operand value for the instruction given, adjusting how it is fetched based on the addressing of the instruction

            IRegisters r = cpu.Registers;
            address = 0x0000;

            byte value;
            ByteRegister source = instruction.Source.AsByteRegister();
            if (source != ByteRegister.None)
            {
                // operand comes from another byte register directly (eg LD A,B)
                value = r[source];
            }
            else
            {
                if (instruction.Argument1 == InstructionElement.ByteValue)
                {
                    // operand is supplied as an argument (eg LD A,n)
                    value = data.Argument1;
                }
                else if (instruction.Source == InstructionElement.None)
                {
                    // operand is fetched from a memory location but the source and target are the same (eg INC (HL) or INC (IX+d))
                    address = instruction.Target.AsWordRegister() switch
                    {
                        WordRegister.IX => (ushort)(r.IX + (sbyte)data.Argument1),
                        WordRegister.IY => (ushort)(r.IY + (sbyte)data.Argument1),
                        _ => r.HL
                    };

                    value = cpu.Memory.ReadByteAt(address, memoryReadTStates);
                }
                else
                {
                    // operand is fetched from a memory location and assigned elsewhere (eg LD A,(HL) or LD B,(IX+d))
                    address = instruction.Source.AsWordRegister() switch
                    {
                        WordRegister.IX => (ushort)(r.IX + (sbyte)data.Argument1),
                        WordRegister.IY => (ushort)(r.IY + (sbyte)data.Argument1),
                        _ => r.HL
                    };

                    value = cpu.Memory.ReadByteAt(address, memoryReadTStates);
                }
            }

            return value;
        }

        public static ushort GetSourceWord(Instruction instruction, InstructionData data, Processor cpu, byte memoryReadTStates)
        {
            IRegisters r = cpu.Registers;
            ushort address = 0x0000;

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

                    value = cpu.Memory.ReadWordAt(address, memoryReadTStates);
                }
            }

            return value;
        }

        public static ushort GetSourceAddress(Instruction instruction, Processor cpu, sbyte offset)
        {
            IRegisters r = cpu.Registers;
            ushort address = instruction.Prefix switch
            {
                0xCB => r.HL,
                0xDDCB => (ushort)(r.IX + offset),
                0xFDCB => (ushort)(r.IY + offset),
                _ => (ushort)0xFFFF
            };
            return address;
        }
    }
}

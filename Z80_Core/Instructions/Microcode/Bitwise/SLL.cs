using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SLL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.Target.AsByteRegister();

            void setFlags(byte original)
            {
                flags = FlagLookup.BitwiseFlags(original, BitwiseOperation.ShiftLeft, flags.Carry);
                flags.Carry = original.GetBit(7);
                flags.HalfCarry = false;
                flags.Subtract = false;
            }

            byte original, shifted;
            if (register != ByteRegister.None)
            {
                original = r[register];
                shifted = (byte)((original << 1) + 1); // bit 0 is always set, adding 1 does this quicker than calling SetBit
                setFlags(original);
                r[register] = shifted;
            }
            else
            {
                ushort address = instruction.Prefix switch
                {
                    InstructionPrefix.CB => r.HL,
                    InstructionPrefix.DDCB => (ushort)(r.IX + offset),
                    InstructionPrefix.FDCB => (ushort)(r.IY + offset),
                    _ => (ushort)0xFFFF
                };
                original = cpu.Memory.ReadByteAt(address, false);
                shifted = (byte)((original << 1) + 1);
                setFlags(original);
                cpu.Memory.WriteByteAt(address, shifted, false);
            }

            return new ExecutionResult(package, flags);
        }

        public SLL()
        {
        }
    }
}

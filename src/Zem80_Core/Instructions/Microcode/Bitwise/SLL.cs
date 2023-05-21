using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
{
    public class SLL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            Registers r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.Target.AsByteRegister();

            void setFlags(byte original)
            {
                flags = FlagLookup.BitwiseFlags(original, BitwiseOperation.ShiftLeftSetBit0, flags.Carry);
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
                    0xCB => r.HL,
                    0xDDCB => (ushort)(r.IX + offset),
                    0xFDCB => (ushort)(r.IY + offset),
                    _ => (ushort)0xFFFF
                };
                original = cpu.Memory.Timed.ReadByteAt(address);
                shifted = (byte)((original << 1) + 1);
                setFlags(original);
                cpu.Memory.Timed.WriteByteAt(address, shifted);
                if (instruction.CopyResultTo != ByteRegister.None)
                {
                    r[instruction.CopyResultTo.Value] = shifted;
                }
            }

            return new ExecutionResult(package, flags);
        }

        public SLL()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class RR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.Target.AsByteRegister();
            bool previousCarry = flags.Carry;

            byte original, shifted;
            if (register != ByteRegister.None)
            {
                original = r.Direct[register];
                shifted = (byte)(original >> 1);
                shifted = shifted.SetBit(7, previousCarry);
                setFlags(original, shifted, original.GetBit(0));
                r.Direct[register] = shifted;
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
                original = cpu.Memory.Timed.ReadByteAt(address);
                shifted = (byte)(original >> 1);
                shifted = shifted.SetBit(7, previousCarry);
                setFlags(original, shifted, original.GetBit(0));
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(4);
                cpu.Memory.Timed.WriteByteAt(address, shifted);
                if (instruction.CopyResultTo != ByteRegister.None)
                {
                    r.Direct[instruction.CopyResultTo.Value] = shifted;
                }
            }

            void setFlags(byte original, byte shifted, bool overflowBit)
            {
                flags = FlagLookup.BitwiseFlags(original, BitwiseOperation.RotateRightThroughCarry, previousCarry);
                flags.Carry = overflowBit;
                flags.HalfCarry = false;
                flags.Subtract = false;
            }

            return new ExecutionResult(package, flags);
        }

        public RR()
        {
        }
    }
}

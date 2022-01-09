using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class SRL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags;
            Registers r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.Target.AsByteRegister();

            void setFlags(byte original)
            {
                flags = FlagLookup.BitwiseFlags(original, BitwiseOperation.ShiftRight, flags.Carry);
                flags.Carry = original.GetBit(0);
                flags.HalfCarry = false;
                flags.Subtract = false;
            }

            byte original, shifted;
            if (register != ByteRegister.None)
            {
                original = r[register];
                shifted = (byte)(original >> 1);
                shifted = shifted.SetBit(7, false);
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
                shifted = (byte)(original >> 1);
                shifted = shifted.SetBit(7, false);
                setFlags(original);
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(4);
                cpu.Memory.Timed.WriteByteAt(address, shifted);
                if (instruction.CopyResultTo != ByteRegister.None)
                {
                    r[instruction.CopyResultTo.Value] = shifted;
                }
            }

            return new ExecutionResult(package, flags);
        }

        public SRL()
        {
        }
    }
}

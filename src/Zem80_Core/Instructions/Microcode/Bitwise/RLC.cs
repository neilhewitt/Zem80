using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class RLC : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.Target.AsByteRegister();

            byte original, shifted;
            if (register != ByteRegister.None)
            {
                original = r[register];
                shifted = (byte)(original << 1);
                shifted = shifted.SetBit(0, original.GetBit(7));
                setFlags(original, shifted);
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
                original = cpu.Memory.Timed.ReadByteAt(address);
                shifted = (byte)(original << 1);
                shifted = shifted.SetBit(0, original.GetBit(7));
                setFlags(original, shifted);
                if (instruction.IsIndexed) cpu.InstructionTiming.InternalOperationCycle(4);
                cpu.Memory.Timed.WriteByteAt(address, shifted);
            }

            void setFlags(byte original, byte shifted)
            {
                flags = FlagLookup.BitwiseFlags(original, BitwiseOperation.RotateLeft, flags.Carry);
                flags.Carry = original.GetBit(7);
                flags.HalfCarry = false;
                flags.Subtract = false;
            }

            return new ExecutionResult(package, flags);
        }

        public RLC()
        {
        }
    }
}

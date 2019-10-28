using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RL : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IFlags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            RegisterIndex index = data.RegisterIndex ?? RegisterIndex.None;
            bool previousCarry = flags.Carry;

            byte setFlags(byte original, byte shifted)
            {
                flags.Carry = (original & 0x80) == 0x80;
                if (((sbyte)shifted) < 0) flags.Sign = true;
                if (shifted == 0) flags.Zero = true;
                if (shifted.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
                if (previousCarry) shifted = (byte)(shifted & 0x01);
                return shifted;
            }

            byte original, shifted;
            if (index != RegisterIndex.None)
            {
                original = r[index];
                shifted = (byte)(original << 1);
                shifted = setFlags(original, shifted);
                r[index] = shifted;
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
                original = cpu.Memory.ReadByteAt(address);
                shifted = (byte)(original << 1);
                shifted = setFlags(original, shifted);
                cpu.Memory.WriteByteAt(address, shifted);
            }

            return new ExecutionResult(flags, 0);
        }

        public RL()
        {
        }
    }
}

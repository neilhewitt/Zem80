using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RRC : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            RegisterName register = data.Register ?? RegisterName.None;

            byte original, shifted;
            if (register != RegisterName.None)
            {
                original = r[register];
                shifted = (byte)(original >> 1);
                shifted = shifted.SetBit(7, original.GetBit(0));
                shifted = setFlags(original, shifted);
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
                original = cpu.Memory.ReadByteAt(address);
                shifted = (byte)(original >> 1);
                shifted = shifted.SetBit(7, original.GetBit(0));
                shifted = setFlags(original, shifted);
                cpu.Memory.WriteByteAt(address, shifted);
            }

            byte setFlags(byte original, byte shifted)
            {
                flags.Carry = shifted.GetBit(7);
                if (((sbyte)shifted) < 0) flags.Sign = true;
                if (shifted == 0) flags.Zero = true;
                if (shifted.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
                return shifted;
            }

            return new ExecutionResult(package, flags, false);
        }

        public RRC()
        {
        }
    }
}

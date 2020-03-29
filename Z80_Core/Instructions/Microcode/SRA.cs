using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SRA : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.OperandByteRegister;

            void setFlags(byte original, byte shifted)
            {
                flags = FlagLookup.BitwiseFlags(original, BitwiseOperation.ShiftRight);
                flags.Carry = shifted.GetBit(0);
                flags.HalfCarry = false;
                flags.Subtract = false;
            }

            byte original, shifted;
            if (register != ByteRegister.None)
            {
                original = r[register];
                shifted = (byte)(original >> 1);
                shifted = shifted.SetBit(7, original.GetBit(7));
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
                original = cpu.Memory.ReadByteAt(address);
                shifted = (byte)(original >> 1);
                shifted = shifted.SetBit(7, original.GetBit(7));
                setFlags(original, shifted);
                cpu.Memory.WriteByteAt(address, shifted);
            }

            return new ExecutionResult(package, flags, false, false);
        }

        public SRA()
        {
        }
    }
}

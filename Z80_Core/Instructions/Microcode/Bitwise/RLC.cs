using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RLC : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.OperandRegister;

            byte original, shifted;
            if (register != ByteRegister.None)
            {
                original = r[register];
                shifted = (byte)(original << 1);
                shifted = shifted.SetBit(0, original.GetBit(7));
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
                original = cpu.Memory.ReadByteAt(address, false);
                shifted = (byte)(original << 1);
                shifted = shifted.SetBit(0, original.GetBit(7));
                shifted = setFlags(original, shifted);
                if (instruction.HLIX || instruction.HLIY) cpu.NotifyInternalOperationCycle(4);
                cpu.Memory.WriteByteAt(address, shifted, false);
            }

            byte setFlags(byte original, byte shifted)
            {
                flags = FlagLookup.BitwiseFlags(original, BitwiseOperation.RotateLeft);
                flags.HalfCarry = false;
                flags.Subtract = false;
                return shifted;
            }

            return new ExecutionResult(package, flags, false, false);
        }

        public RLC()
        {
        }
    }
}

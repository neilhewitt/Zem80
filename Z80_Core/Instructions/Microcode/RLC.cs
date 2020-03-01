using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RLC : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            RegisterName register = instruction.OperandRegister;

            byte original, shifted;
            if (register != RegisterName.None)
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
                original = cpu.Memory.ReadByteAt(address);
                shifted = (byte)(original << 1);
                shifted = shifted.SetBit(0, original.GetBit(7));
                shifted = setFlags(original, shifted);
                cpu.Memory.WriteByteAt(address, shifted);
            }

            byte setFlags(byte original, byte shifted)
            {
                FlagHelper.SetFlagsFromLogicalOperation(flags, original, 0x00, shifted, shifted.GetBit(0),
                    new Flag[] { Flag.HalfCarry, Flag.Subtract });
                flags.HalfCarry = false;
                flags.Subtract = false;
                return shifted;
            }

            return new ExecutionResult(package, flags, false);
        }

        public RLC()
        {
        }
    }
}

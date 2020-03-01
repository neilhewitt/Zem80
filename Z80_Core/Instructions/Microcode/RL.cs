using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;

            sbyte offset = (sbyte)(data.Argument1);
            RegisterName register = instruction.OperandRegister;
            bool previousCarry = flags.Carry;

            byte original, shifted;
            if (register != RegisterName.None)
            {
                original = r[register];
                shifted = (byte)(original << 1);
                shifted = flagsAndCarry(original, shifted);
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
                shifted = flagsAndCarry(original, shifted);
                cpu.Memory.WriteByteAt(address, shifted);
            }

            byte flagsAndCarry(byte original, byte shifted)
            {
                shifted = shifted.SetBit(0, previousCarry);
                FlagHelper.SetFlagsFromLogicalOperation(flags, original, 0x00, shifted, original.GetBit(7),
                    new Flag[] { Flag.HalfCarry, Flag.Subtract });
                flags.HalfCarry = false;
                flags.Subtract = false;
                return shifted;
            }

            return new ExecutionResult(package, flags, false);
        }

        public RL()
        {
        }
    }
}

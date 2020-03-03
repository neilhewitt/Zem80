using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SRL : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            RegisterName register = instruction.OperandRegister;

            byte setFlags(byte original, byte shifted)
            {
                flags = FlagLookup.FlagsFromBitwiseOperation(original, BitwiseOperation.ShiftRight);
                flags.HalfCarry = false;
                flags.Subtract = false;
                return shifted;
            }

            byte original, shifted;
            if (register != RegisterName.None)
            {
                original = r[register];
                shifted = (byte)(original >> 1);
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
                shifted = setFlags(original, shifted);
                cpu.Memory.WriteByteAt(address, shifted);
            }

            return new ExecutionResult(package, flags, false);
        }

        public SRL()
        {
        }
    }
}

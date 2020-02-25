using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class BIT : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            Flags flags = new Flags();
            InstructionPrefix prefix = instruction.Prefix;

            byte bitIndex = data.BitIndex.Value;
            byte value;
            if (data.Register.HasValue)
            {
                value = r[data.Register ?? RegisterName.None]; // BIT b, r
            }
            else
            {
                ushort address = data.IndexIX ? r.IX : data.IndexIY ? r.IY : r.HL; // BIT b, (HL / IX+o / IY+o)
                sbyte offset = (data.IndexIY || data.IndexIY) ? (sbyte)data.Argument1 : (sbyte)0;
                value = cpu.Memory.ReadByteAt((ushort)(address + offset));
            }

            flags.Zero = !value.GetBit(bitIndex);
            flags.HalfCarry = true;
            flags.Carry = r.Flags.Carry;

            return new ExecutionResult(package, flags, false);
        }

        public BIT()
        {
        }
    }
}

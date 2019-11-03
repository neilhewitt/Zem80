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
            if (data.RegisterIndex.HasValue)
            {
                value = r[data.RegisterIndex ?? RegisterIndex.None]; // BIT b, r
            }
            else
            {
                ushort address = data.IndexIX ? r.IX : data.IndexIY ? r.IY : r.HL; // BIT b, (HL / IX+o / IY+o)
                byte offset = (data.IndexIY || data.IndexIY) ? data.Argument1 : (byte)0;
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

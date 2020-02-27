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

            byte bitIndex = instruction.BitIndex.Value;
            byte value;
            if (instruction.OperandRegister != RegisterName.None)
            {
                value = r[instruction.OperandRegister]; // BIT b, r
            }
            else
            {
                ushort address = instruction.IndexIX ? r.IX : instruction.IndexIY ? r.IY : r.HL; // BIT b, (HL / IX+o / IY+o)
                sbyte offset = (instruction.IndexIY || instruction.IndexIY) ? (sbyte)data.Argument1 : (sbyte)0;
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

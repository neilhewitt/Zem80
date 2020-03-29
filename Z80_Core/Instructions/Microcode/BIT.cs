using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class BIT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            Flags flags = cpu.Registers.Flags;
            InstructionPrefix prefix = instruction.Prefix;

            byte bitIndex = instruction.BitIndex.Value;
            byte value;
            if (instruction.OperandByteRegister != ByteRegister.None)
            {
                value = r[instruction.OperandByteRegister]; // BIT b, r
            }
            else
            {
                ushort address = instruction.ReplacesHLWithIX ? r.IX : instruction.ReplacesHLWithIY ? r.IY : r.HL; // BIT b, (HL / IX+o / IY+o)
                sbyte offset = (instruction.ReplacesHLWithIY || instruction.ReplacesHLWithIY) ? (sbyte)data.Argument1 : (sbyte)0;
                value = cpu.Memory.ReadByteAt((ushort)(address + offset));
            }

            flags.Zero = value.GetBit(bitIndex) == false;
            flags.HalfCarry = true;
            flags.Subtract = false;

            return new ExecutionResult(package, flags, false, false);
        }

        public BIT()
        {
        }
    }
}

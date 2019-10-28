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
            byte value = prefix == InstructionPrefix.CB ? 
                r[data.RegisterIndex ?? RegisterIndex.None] : // BIT b, r
                cpu.Memory.ReadByteAt((ushort)((prefix == InstructionPrefix.DDCB ? r.IX : r.IY) + data.Argument1)); // bit b, (HL)

            flags.Zero = !value.GetBit(bitIndex);
            flags.HalfCarry = true;
            flags.Carry = r.Flags.Carry;

            return new ExecutionResult(flags, 0);
        }

        public BIT()
        {
        }
    }
}

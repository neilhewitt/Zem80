using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RLA : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IFlags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;

            byte value = r.A;
            bool carry = ((byte)(value & 0x80) == 0x80);
            value = (byte)(value << 1);
            if (flags.Carry) value = (byte)(value | 0x01);
            flags.Carry = carry;
            r.A = value;

            return new ExecutionResult(flags, 0);
        }

        public RLA()
        {
        }
    }
}

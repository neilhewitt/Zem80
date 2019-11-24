using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RRA : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IFlags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;

            byte value = r.A;
            bool carry = ((byte)(value & 0x01) == 0x01);
            value = (byte)(value >> 1);
            if (flags.Carry) value = (byte)(value | 0x80);
            flags.Carry = carry;
            r.A = value;

            return new ExecutionResult(package, flags, false);
        }

        public RRA()
        {
        }
    }
}

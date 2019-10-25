using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RLCA : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;

            byte value = r.A;
            flags.Carry = (value & 0x80) == 0x80;
            value = (byte)(value << 1);
            if (flags.Carry) value = (byte)(value | 0x01);
            r.A = value;

            return new ExecutionResult(flags, 0);
        }

        public RLCA()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RRCA : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;

            byte value = r.A;
            flags.Carry = (value & 0x01) == 0x01;
            value = (byte)(value >> 1);
            if (flags.Carry) value = (byte)(value | 0x80);
            r.A = value;

            return new ExecutionResult(package, flags, false);
        }

        public RRCA()
        {
        }
    }
}

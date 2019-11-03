using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class NEG : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            Flags flags = new Flags();

            sbyte result = (sbyte)(0 - r.A);
            if (result == 0x00) flags.Zero = true;
            if (result < 0) flags.Sign = true;
            if (((result & 0x0F) & 0x10) != 0x10) flags.HalfCarry = true;
            if (r.A == 0x80) flags.ParityOverflow = true;
            if (r.A != 0x00) flags.Carry = true;
            flags.Subtract = true;

            r.A = (byte)result;

            return new ExecutionResult(package, flags, false);
        }

        public NEG()
        {
        }
    }
}

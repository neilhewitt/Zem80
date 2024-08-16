using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class NEG : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            IRegisters r = cpu.Registers;

            int result = 0x00 - r.A;
            Flags flags = FlagLookup.ByteArithmeticFlags(0x00, r.A, false, true);
            flags.ParityOverflow = r.A == 0x80;
            flags.Carry = r.A != 0x00;
            r.A = (byte)result;

            return new ExecutionResult(package, flags);
        }

        public NEG()
        {
        }
    }
}

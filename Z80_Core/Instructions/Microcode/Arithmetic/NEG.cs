using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class NEG : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            Flags flags = cpu.Registers.Flags;

            int result = 0x00 - r.A;
            flags = FlagLookup.ByteArithmeticFlags(0x00, r.A, false, true);
            flags.ParityOverflow = r.A == 0x80;
            flags.Carry = r.A != 0x00;
            r.A = (byte)result;

            return new ExecutionResult(package, flags, false, false);
        }

        public NEG()
        {
        }
    }
}

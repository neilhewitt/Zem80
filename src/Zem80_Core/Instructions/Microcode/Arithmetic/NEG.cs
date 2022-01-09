using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class NEG : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            Flags flags = cpu.Flags;

            int result = 0x00 - r.A;
            flags = FlagLookup.ByteArithmeticFlags(0x00, r.A, false, true);
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

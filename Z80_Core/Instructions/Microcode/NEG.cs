using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class NEG : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            Flags flags = cpu.Registers.Flags;

            int result = 0x00 - r.A;
            FlagHelper.SetFlagsFromArithmeticOperation(flags, 0x00, r.A, result, true);
            r.A = (byte)result;

            return new ExecutionResult(package, flags, false);
        }

        public NEG()
        {
        }
    }
}

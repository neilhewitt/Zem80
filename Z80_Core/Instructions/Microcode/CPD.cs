using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CPD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            byte a = cpu.Registers.A;
            byte b = cpu.Memory.ReadByteAt(cpu.Registers.HL);
            int result = a - b;
            cpu.Registers.HL--;
            cpu.Registers.BC--;

            FlagHelper.SetFlagsFromArithmeticOperation(flags, a, b, result, true);
            flags.ParityOverflow = (cpu.Registers.BC - 1 != 0);

            return new ExecutionResult(package, flags, false);
        }

        public CPD()
        {
        }
    }
}

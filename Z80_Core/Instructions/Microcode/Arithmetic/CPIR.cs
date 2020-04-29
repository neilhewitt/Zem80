using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CPIR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            byte a = cpu.Registers.A;
            byte b = cpu.Memory.ReadByteAt(cpu.Registers.HL, false);
            int result = a - b;
            cpu.Registers.HL++;
            cpu.Registers.BC--;

            flags = FlagLookup.ByteArithmeticFlags(a, b, false, true);
            flags.ParityOverflow = (cpu.Registers.BC - 1 != 0);

            bool conditionTrue = (result == 0 || cpu.Registers.BC == 0);
            if (conditionTrue) cpu.NotifyInternalOperationCycle(5);

            return new ExecutionResult(package, flags, conditionTrue, !conditionTrue);
        }

        public CPIR()
        {
        }
    }
}

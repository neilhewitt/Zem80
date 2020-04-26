using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class LDIR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            cpu.Memory.WriteByteAt(r.DE, cpu.Memory.ReadByteAt(r.HL));
            r.HL++;
            r.DE++;
            r.BC--;

            flags.HalfCarry = false;
            flags.ParityOverflow = false;
            flags.Subtract = false;

            bool conditionTrue = (r.BC == 0);
            if (conditionTrue) cpu.InternalOperationCycle(5);

            return new ExecutionResult(package, flags, conditionTrue, !conditionTrue);
        }

        public LDIR()
        {
        }
    }
}

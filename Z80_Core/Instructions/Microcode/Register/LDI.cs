using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class LDI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            cpu.Memory.WriteByteAt(r.DE, cpu.Memory.ReadByteAt(r.HL, false), false);
            r.HL++;
            r.DE++;
            r.BC--;

            flags.HalfCarry = false;
            flags.ParityOverflow = (r.BC != 0);
            flags.Subtract = false;

            return new ExecutionResult(package, flags, false, false);
        }

        public LDI()
        {
        }
    }
}

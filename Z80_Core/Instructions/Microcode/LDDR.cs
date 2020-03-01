using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class LDDR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;

            cpu.Memory.WriteByteAt(r.DE, cpu.Memory.ReadByteAt(r.HL));
            r.HL--;
            r.DE--;
            r.BC--;

            flags.HalfCarry = false;
            flags.ParityOverflow = false;
            flags.Subtract = false;

            return new ExecutionResult(package, flags, (r.BC == 0), (r.BC != 0));
        }

        public LDDR()
        {
        }
    }
}

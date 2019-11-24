using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class LDIR : IInstructionImplementation
    {
        public ExecutionResult Execute(IProcessor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;

            cpu.Memory.WriteByteAt(r.DE, cpu.Memory.ReadByteAt(r.HL));
            r.HL++;
            r.DE++;
            r.BC--;

            if (r.BC != 0) flags.ParityOverflow = true;

            return new ExecutionResult(package, flags, (r.BC == 0), (r.BC != 0));
        }

        public LDIR()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class LDD : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;

            cpu.Memory.WriteByteAt(r.DE, cpu.Memory.ReadByteAt(r.HL));
            r.HL--;
            r.DE--;
            r.BC--;

            if (r.BC != 0) flags.ParityOverflow = true;

            return new ExecutionResult(flags, 0);
        }

        public LDD()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CPDR : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();

            byte a = cpu.Registers.A;
            byte b = cpu.Memory.ReadByteAt(cpu.Registers.HL);
            short result = (short)(a - b);
            cpu.Registers.HL--;
            cpu.Registers.BC--;

            if (result < 0) flags.Sign = true;
            if (result == 0) flags.Zero = true;
            if ((a & 0xF) < (b & 0xF)) flags.HalfCarry = true;
            if (cpu.Registers.BC == 0) flags.ParityOverflow = true;
            flags.Subtract = true;

            return new ExecutionResult(package, flags, (result == 0 || cpu.Registers.BC == 0), (result != 0 && cpu.Registers.BC != 0));
        }

        public CPDR()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CPI : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();

            byte a = cpu.Registers.A;
            byte b = cpu.Memory.ReadByteAt(cpu.Registers.HL);
            short result = (short)(a - b);
            cpu.Registers.HL++;
            cpu.Registers.BC--;

            if ((sbyte)result < 0) flags.Sign = true;
            if (result == 0) flags.Zero = true;
            if (a.HalfCarryWhenSubtracting(b)) flags.HalfCarry = true;
            if (cpu.Registers.BC != 0) flags.ParityOverflow = true;
            flags.Subtract = true;

            return new ExecutionResult(package, flags, false);
        }

        public CPI()
        {
        }
    }
}

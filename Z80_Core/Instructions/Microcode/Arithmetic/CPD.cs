using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class CPD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            byte a = cpu.Registers.A;
            byte b = cpu.Memory.ReadByteAt(cpu.Registers.HL, false);
            cpu.Registers.HL--;
            cpu.Registers.BC--;

            flags.Sign = (byte)(a - b) < 0;
            flags.Zero = a - b == 0;
            flags.HalfCarry = FlagLookup.HalfCarry(a, b, false, true);
            flags.ParityOverflow = ((ushort)(cpu.Registers.BC - 1) != 0);
            flags.Subtract = true;

            return new ExecutionResult(package, flags);
        }

        public CPD()
        {
        }
    }
}

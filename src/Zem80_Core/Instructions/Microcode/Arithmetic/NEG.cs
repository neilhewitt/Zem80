using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class NEG : MicrocodeBase
    {
        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            IRegisters r = cpu.Registers;
            Flags flags = cpu.Flags.Clone();

            int result = 0x00 - r.A;

            flags.Zero = ((byte)result == 0);
            flags.Sign = ((sbyte)result < 0);
            flags.HalfCarry = ((0x00 ^ (byte)(result & 0xFF) ^ r.A) & 0x10) != 0;
            flags.Subtract = true; // don't forget to override
            flags.ParityOverflow = r.A == 0x80;
            flags.Carry = r.A != 0x00;
            flags.X = (result & 0x08) > 0; // copy bit 3
            flags.Y = (result & 0x20) > 0; // copy bit 5

            r.A = (byte)result;

            return new ExecutionResult(package, flags);
        }

        public NEG()
        {
        }
    }
}

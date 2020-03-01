using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RLD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            byte xHL = cpu.Memory.ReadByteAt(cpu.Registers.HL);
            byte a = cpu.Registers.A;

            // result = (HL) = LO: high-order bits of (HL) + HI: high-order bits of A
            // A = LO: low-order bits of (HL) + HI: low-order bits of A

            bool[] lowA = a.GetLowNybble();
            a = a.SetLowNybble(xHL.GetHighNybble());
            xHL = xHL.SetHighNybble(xHL.GetLowNybble());
            xHL = xHL.SetLowNybble(lowA);

            cpu.Memory.WriteByteAt(cpu.Registers.HL, xHL);
            cpu.Registers.A = a;

            FlagHelper.SetFlagsFromLogicalOperation(flags, 0x00, 0x00, a, false,
                new Flag[] { Flag.HalfCarry, Flag.Subtract, Flag.Carry });
            flags.HalfCarry = false;
            flags.Subtract = false;

            return new ExecutionResult(package, cpu.Registers.Flags, false);
        }

        public RLD()
        {
        }
    }
}

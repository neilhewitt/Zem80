using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RLA : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;

            byte value = r.A;
            bool carry = value.GetBit(7);
            value = (byte)(value << 1);
            value = value.SetBit(0, flags.Carry);
            flags.Carry = carry;
            flags.HalfCarry = false;
            flags.Subtract = false;

            r.A = value;

            return new ExecutionResult(package, flags, false);
        }

        public RLA()
        {
        }
    }
}

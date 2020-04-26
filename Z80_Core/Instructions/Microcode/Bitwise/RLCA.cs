using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RLCA : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            byte value = r.A;
            flags.Carry = value.GetBit(7);
            value = (byte)(value << 1);
            value = value.SetBit(0, flags.Carry);
            flags.HalfCarry = false;
            flags.Subtract = false;
            r.A = value;

            return new ExecutionResult(package, flags, false, false);
        }

        public RLCA()
        {
        }
    }
}
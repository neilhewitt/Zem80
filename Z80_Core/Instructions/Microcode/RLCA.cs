using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RLCA : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;

            byte value = r.A;
            flags.Carry = value.GetBit(7);
            value = (byte)(value << 1);
            value = value.SetBit(0, flags.Carry);
            r.A = value;

            return new ExecutionResult(package, flags, false);
        }

        public RLCA()
        {
        }
    }
}

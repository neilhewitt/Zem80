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

            bool carry = flags.Carry;
            byte a = cpu.Registers.A;
            byte b = cpu.Memory.ReadByteAt(cpu.Registers.HL, false);

            var compare = ALUOperations.Subtract(a, b, false);
            flags = compare.Flags;
            flags.Carry = carry;

            cpu.Registers.BC--;
            flags.ParityOverflow = (cpu.Registers.BC != 0);

            cpu.Registers.HL--;

            flags.Subtract = true;
            flags.Carry = carry;

            return new ExecutionResult(package, flags);
        }

        public CPD()
        {
        }
    }
}

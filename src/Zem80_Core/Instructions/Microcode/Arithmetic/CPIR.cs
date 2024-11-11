using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class CPIR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            bool carry = cpu.Flags.Carry;
            byte a = cpu.Registers.A;
            byte b = cpu.Memory.ReadByteAt(cpu.Registers.HL, 3);

            (byte result, Flags flags) = Arithmetic.Subtract(a, b, false);

            cpu.Registers.BC--;
            flags.ParityOverflow = (cpu.Registers.BC != 0);
            cpu.Registers.HL++;

            flags.Subtract = true;
            flags.Carry = carry;
            byte valueXY = (byte)(a - b - (flags.HalfCarry ? 1 : 0));
            flags.X = (valueXY & 0x08) > 0; // copy bit 3
            flags.Y = (valueXY & 0x02) > 0; // copy bit 1 (note: non-standard behaviour)

            cpu.Registers.WZ++;
            cpu.Timing.InternalOperationCycle(5);

            if (flags.Zero || !flags.ParityOverflow)
            {
                cpu.Timing.InternalOperationCycle(5);
            }
            else
            {
                cpu.Registers.PC = package.InstructionAddress;
                cpu.Registers.WZ = (ushort)(cpu.Registers.PC + 1);
            }

            return new ExecutionResult(package, flags);
        }

        public CPIR()
        {
        }
    }
}

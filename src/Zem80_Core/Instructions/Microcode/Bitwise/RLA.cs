using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RLA : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            byte value = r.A;
            (value, flags) = BitwiseOperations.RotateLeftThroughCarry(value, flags, FlagState.Carry | FlagState.HalfCarry | FlagState.Subtract | FlagState.X | FlagState.Y);
            r.A = value;

            return new ExecutionResult(package, flags);
        }

        public RLA()
        {
        }
    }
}

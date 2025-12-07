using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RRCA : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            byte value = r.A;
            (value, flags) = Bitwise.RotateRight(value, flags, FlagState.Carry | FlagState.HalfCarry | FlagState.Subtract | FlagState.X | FlagState.Y);
            r.A = value;

            return new ExecutionResult(package, flags);
        }

        public RRCA()
        {
        }
    }
}

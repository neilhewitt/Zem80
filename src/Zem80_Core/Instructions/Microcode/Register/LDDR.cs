using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class LDDR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            byte value = cpu.Memory.ReadByteAt(r.HL, false);
            cpu.Memory.WriteByteAt(r.DE, value, false);
            r.HL--;
            r.DE--;
            r.BC--;

            flags.HalfCarry = false;
            flags.ParityOverflow = (r.BC != 0);
            flags.Subtract = false;
            flags.X = (((byte)(value + cpu.Registers.A)) & 0x08) > 0; // copy bit 3
            flags.Y = (((byte)(value + cpu.Registers.A)) & 0x02) > 0; // copy bit 1 (note: non-standard behaviour)

            bool conditionTrue = (r.BC == 0);
            if (conditionTrue) cpu.Timing.InternalOperationCycle(5);
            else r.PC = package.InstructionAddress;

            return new ExecutionResult(package, flags);
        }

        public LDDR()
        {
        }
    }
}
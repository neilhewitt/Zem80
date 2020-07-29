using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class BIT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            Flags flags = cpu.Registers.Flags;
            InstructionPrefix prefix = instruction.Prefix;

            byte bitIndex = instruction.GetBitIndex();
            byte value;
            ByteRegister register = instruction.Target.AsByteRegister();
            if (register != ByteRegister.None)
            {
                value = r[register]; // BIT b, r
            }
            else
            {
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                value = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);
            }

            flags.Sign = ((sbyte)(value)) < 0;
            flags.Zero = value.GetBit(bitIndex) == false;
            flags.HalfCarry = true;
            flags.Subtract = false;

            return new ExecutionResult(package, flags);
        }

        public BIT()
        {
        }
    }
}

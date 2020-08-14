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
            ByteRegister register = instruction.Source.AsByteRegister();
            if (register != ByteRegister.None)
            {
                value = r[register]; // BIT b, r
                flags.X = (value & 0x08) > 0; // copy bit 3
                flags.Y = (value & 0x20) > 0; // copy bit 5
            }
            else
            {
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                value = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);
                byte valueXY = address.HighByte();
                flags.X = (valueXY & 0x08) > 0; // copy bit 3
                flags.Y = (valueXY & 0x20) > 0; // copy bit 5
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

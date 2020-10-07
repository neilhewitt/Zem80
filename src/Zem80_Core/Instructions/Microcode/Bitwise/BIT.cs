using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
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
            }
            else
            {
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                value = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);
                byte valueXY = instruction.IsIndexed ? address.HighByte() : r.HL.HighByte();
                flags.X = (valueXY & 0x08) > 0; // copy bit 3
                flags.Y = (valueXY & 0x20) > 0; // copy bit 5
            }

            bool set = value.GetBit(bitIndex);

            flags.Sign = bitIndex == 7 && set;
            flags.Zero = !set;
            flags.ParityOverflow = flags.Zero;
            flags.HalfCarry = true;
            flags.Subtract = false;

            return new ExecutionResult(package, flags);
        }

        public BIT()
        {
        }
    }
}
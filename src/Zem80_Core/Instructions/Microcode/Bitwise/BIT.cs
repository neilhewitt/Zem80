using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class BIT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            Flags flags = cpu.Flags.Clone();

            byte bitIndex = instruction.GetBitIndex();
            byte value;
            bool set;
            ByteRegister register = instruction.Source.AsByteRegister();

            if (register != ByteRegister.None)
            {
                value = r[register]; // BIT b, r
                set = value.GetBit(bitIndex);
                flags.X = (value & 0x08) > 0; // copy bit 3
                flags.Y = (value & 0x20) > 0; // copy bit 5
            }
            else // BIT b,(IX/IY+d) or BIT b,(HL)
            {
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                value = instruction.MarshalSourceByte(data, cpu, out ushort address, 4);
                set = value.GetBit(bitIndex);
                byte valueXY = instruction.IsIndexed ? address.HighByte() : r.WZ.HighByte(); // this is literally the only place the WZ value is *ever* actually used
                flags.X = (valueXY & 0x08) > 0; // copy bit 3
                flags.Y = (valueXY & 0x20) > 0; // copy bit 5
            }

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

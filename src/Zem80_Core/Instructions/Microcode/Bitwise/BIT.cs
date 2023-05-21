using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.Instructions
{
    public class BIT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            Flags flags = cpu.Flags.Clone();

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
                value = instruction.MarshalSourceByte(data, cpu, out ushort address);
                byte valueXY = instruction.IsIndexed ? address.HighByte() : r.WZ.HighByte(); // this is literally the only place the WZ value is *ever* actually used
                flags.X = (valueXY & 0x08) > 0; // copy bit 3
                flags.Y = (valueXY & 0x20) > 0; // copy bit 5
            }

            bool set = value.GetBit(bitIndex);

            flags.Sign = bitIndex == 7 && set;
            
            if (register == ByteRegister.None)
            {
                flags.Y = bitIndex == 5 && set;
                flags.X = bitIndex == 3 && set;
            }
            
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

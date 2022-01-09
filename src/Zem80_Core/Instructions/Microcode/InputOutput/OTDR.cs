using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.IO;

namespace Zem80.Core.Instructions
{
    public class OTDR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags;
            Registers r = cpu.Registers;

            Port port = cpu.Ports[r.C];
            byte output = cpu.Memory.Timed.ReadByteAt(r.HL);
            r.WZ = r.BC;
            r.B--;
            port.SignalWrite();
            port.WriteByte(output, true);
            r.HL--;

            flags.Zero = true;
            flags.Subtract = true;
            flags.X = (output & 0x08) > 0; // copy bit 3
            flags.Y = (output & 0x20) > 0; // copy bit 5

            if (r.B != 0) r.PC = package.InstructionAddress;

            return new ExecutionResult(package, flags);
        }

        public OTDR()
        {
        }
    }
}

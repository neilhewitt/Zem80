using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.IO;

namespace Zem80.Core.Instructions
{
    public class OUTD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = cpu.Flags.Clone();
            Registers r = cpu.Registers;

            Port port = cpu.Ports[r.C];
            byte output = cpu.Memory.Timed.ReadByteAt(r.HL);
            r.WZ = r.BC;
            r.B--;
            port.SignalWrite();
            port.WriteByte(output, true);
            r.HL--;

            flags.Zero = (r.B == 0);
            flags.Subtract = true;
            flags.X = (output & 0x08) > 0; // copy bit 3
            flags.Y = (output & 0x20) > 0; // copy bit 5

            return new ExecutionResult(package, flags);
        }

        public OUTD()
        {
        }
    }
}

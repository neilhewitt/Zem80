using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.IO;

namespace Zem80.Core.Instructions
{
    public class IND : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = cpu.Flags.Clone();
            Registers r = cpu.Registers;

            Port port = cpu.Ports[r.C];
            port.SignalRead();
            byte input = port.ReadByte(true);
            cpu.Memory.Timed.WriteByteAt(r.HL, input);
            r.HL--;
            r.WZ = r.BC;
            r.B--;

            flags.Zero = (r.B == 0);
            flags.Subtract = true;
            flags.X = (input & 0x08) > 0; // copy bit 3
            flags.Y = (input & 0x20) > 0; // copy bit 5

            return new ExecutionResult(package, flags);
        }

        public IND()
        {
        }
    }
}

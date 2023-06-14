using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.InputOutput;

namespace Zem80.Core.CPU
{
    public class INI : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            IPort port = cpu.Ports[r.C];
            port.SignalRead();
            byte input = port.ReadByte(true);
            cpu.Memory.WriteByteAt(r.HL, input, 3);
            r.HL++;
            r.WZ = r.BC;
            r.B--;

            flags.Zero = (r.B == 0);
            flags.Subtract = true;
            flags.X = (input & 0x08) > 0; // copy bit 3
            flags.Y = (input & 0x20) > 0; // copy bit 5

            return new ExecutionResult(package, flags);
        }

        public INI()
        {
        }
    }
}

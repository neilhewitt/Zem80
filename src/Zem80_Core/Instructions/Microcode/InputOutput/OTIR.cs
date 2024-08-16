using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.InputOutput;

namespace Zem80.Core.CPU
{
    public class OTIR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            IPort port = cpu.Ports[r.C];
            byte output = cpu.Memory.ReadByteAt(r.HL, 3);
            r.WZ = r.BC;
            r.B--;
            port.SignalWrite();
            port.WriteByte(output, true);
            r.HL++;

            flags.Zero = true;
            flags.Subtract = true;
            flags.X = (output & 0x08) > 0; // copy bit 3
            flags.Y = (output & 0x20) > 0; // copy bit 5

            if (r.B != 0) r.PC = package.InstructionAddress;

            return new ExecutionResult(package, flags);
        }

        public OTIR()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.InputOutput;

namespace Zem80.Core.CPU
{
    public class OUTD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            IPort port = cpu.Ports[r.C];
            byte output = cpu.Memory.ReadByteAt(r.HL, 3);
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

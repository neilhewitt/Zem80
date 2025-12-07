using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.InputOutput;

namespace Zem80.Core.CPU
{
    public class INIR : MicrocodeBase
    {
        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            IPort port = cpu.Ports[r.C];
            port.SignalRead();
            byte input = port.ReadByte(true);
            cpu.Memory.WriteByteAt(r.HL, input, 3);
            cpu.Timing.InternalOperationCycle(5);
            r.HL++;
            r.WZ = r.BC;
            r.B--;

            flags.Zero = true;
            flags.Subtract = true;
            flags.X = (input & 0x08) > 0; // copy bit 3
            flags.Y = (input & 0x20) > 0; // copy bit 5

            if (r.B != 0) r.PC = package.InstructionAddress;

            return new ExecutionResult(package, flags);
        }

        public INIR()
        {
        }
    }
}

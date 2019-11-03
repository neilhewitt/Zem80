using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class INDR : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IFlags flags = new Flags();
            IRegisters r = cpu.Registers;

            IPort port = cpu.Ports[r.C];
            cpu.SetAddressBus(r.C, r.B);
            port.SignalRead();
            byte input = port.ReadByte();
            cpu.Memory.WriteByteAt(r.HL, input);
            r.HL--;
            r.B--;

            flags.Zero = true;
            flags.Subtract = true;

            return new ExecutionResult(package, flags, (r.B == 0), (r.B != 0));
        }

        public INDR()
        {
        }
    }
}

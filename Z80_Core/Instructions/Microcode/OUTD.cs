using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class OUTD : IInstructionImplementation
    {
        public ExecutionResult Execute(IProcessor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IFlags flags = new Flags();
            IRegisters r = cpu.Registers;

            IPort port = cpu.Ports[r.C];
            byte output = cpu.Memory.ReadByteAt(r.HL);
            r.B--;
            cpu.SetAddressBus(r.C, r.B);
            cpu.SetDataBus(output);
            port.SignalWrite();
            port.WriteByte(output);
            r.HL--;

            if (r.B == 0) flags.Zero = true;
            flags.Subtract = true;

            return new ExecutionResult(package, flags, false);
        }

        public OUTD()
        {
        }
    }
}

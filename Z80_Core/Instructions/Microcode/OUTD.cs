using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class OUTD : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;

            IPort port = cpu.Ports[r.C];
            byte output = cpu.Memory.ReadByteAt(r.HL);
            r.B--;
            cpu.SetAddressBus(r.C, r.B);
            cpu.SetDataBus(output);
            port.SignalWrite();
            port.WriteByte(output);
            r.HL--;

            flags.Zero = (r.B == 0);
            flags.Subtract = true;

            return new ExecutionResult(package, flags, false, false);
        }

        public OUTD()
        {
        }
    }
}

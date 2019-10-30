using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class OTIR : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
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
            r.HL++;

            flags.Zero = true;
            flags.Subtract = true;
            if (r.B != 0) r.PC -= 2;

            return new ExecutionResult(flags, 0, (r.B != 0));
        }

        public OTIR()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class INIR : IInstructionImplementation
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
            r.HL++;
            r.B--;

            if (r.B != 0) r.PC -= 2;

            flags.Zero = true;
            flags.Subtract = true;

            return new ExecutionResult(flags, 0, r.B != 0);
        }

        public INIR()
        {
        }
    }
}

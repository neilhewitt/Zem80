using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class IND : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            IRegisters r = cpu.Registers;

            IPort port = cpu.Ports[r.C];
            cpu.SetAddressBus(r.C, r.B);
            port.SignalRead();
            byte input = port.ReadByte();
            cpu.Memory.WriteByteAt(r.HL, input);
            r.HL--;
            r.B--;

            flags.Zero = (r.B == 0);
            flags.Subtract = true;

            return new ExecutionResult(package, flags, false);
        }

        public IND()
        {
        }
    }
}

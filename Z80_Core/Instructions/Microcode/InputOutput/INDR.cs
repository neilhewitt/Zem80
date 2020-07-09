using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class INDR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            Port port = cpu.Ports[r.C];
            port.SignalRead();
            byte input = port.ReadByte();
            cpu.Memory.WriteByteAt(r.HL, input, false);
            cpu.Timing.InternalOperationCycle(5);
            r.HL--;
            r.B--;

            flags.Sign = false;
            flags.Zero = true;
            flags.Subtract = true;

            return new ExecutionResult(package, flags);
        }

        public INDR()
        {
        }
    }
}

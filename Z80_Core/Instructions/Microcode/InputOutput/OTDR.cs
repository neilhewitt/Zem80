using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class OTDR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            Port port = cpu.Ports[r.C];
            byte output = cpu.Memory.ReadByteAt(r.HL, false);
            r.B--;
            port.SignalWrite();
            port.WriteByte(output);
            r.HL--;

            flags.Zero = true;
            flags.Subtract = true;

            return new ExecutionResult(package, flags);
        }

        public OTDR()
        {
        }
    }
}

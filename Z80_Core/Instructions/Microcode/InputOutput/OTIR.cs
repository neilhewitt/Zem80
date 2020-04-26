using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class OTIR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            Port port = cpu.Ports[r.C];
            byte output = cpu.Memory.ReadByteAt(r.HL);
            r.B--;
            port.SignalWrite();
            port.WriteByte(output);
            r.HL++;

            flags.Zero = true;
            flags.Subtract = true;

            bool conditionTrue = (r.B == 0);

            return new ExecutionResult(package, flags, conditionTrue, !conditionTrue);
        }

        public OTIR()
        {
        }
    }
}

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
            IRegisters r = cpu.Registers;

            IPort port = cpu.Ports[r.C];
            byte output = cpu.Memory.ReadByteAt(r.HL);
            r.B--;
            cpu.SetAddressBus(r.C, r.B);
            cpu.SetDataBus(output);
            port.SignalWrite();
            port.WriteByte(output);
            r.HL--;

            flags.Zero = true;
            flags.Subtract = true;

            bool conditionTrue = (r.B == 0);

            return new ExecutionResult(package, flags, conditionTrue, !conditionTrue);
        }

        public OTDR()
        {
        }
    }
}

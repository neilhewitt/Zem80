using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class INIR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
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
            r.HL++;
            r.B--;

            flags.Zero = true;
            flags.Sign = false;
            flags.Subtract = true;

            bool conditionTrue = (r.B == 0);

            return new ExecutionResult(package, flags, conditionTrue, !conditionTrue);
        }

        public INIR()
        {
        }
    }
}

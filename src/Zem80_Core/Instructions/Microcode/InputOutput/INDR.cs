using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.IO;

namespace Zem80.Core.Instructions
{
    public class INDR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            Port port = cpu.Ports[r.C];
            port.SignalRead();
            byte input = port.ReadByte(true);
            cpu.Memory.Timed.WriteByteAt(r.HL, input);
            cpu.InstructionTiming.InternalOperationCycle(5);
            r.HL--;
            r.WZ = r.BC;
            r.B--;

            flags.Sign = false;
            flags.Zero = true;
            flags.Subtract = true;
            flags.X = (input & 0x08) > 0; // copy bit 3
            flags.Y = (input & 0x20) > 0; // copy bit 5

            if (r.B != 0) r.PC = package.InstructionAddress;

            return new ExecutionResult(package, flags);
        }

        public INDR()
        {
        }
    }
}

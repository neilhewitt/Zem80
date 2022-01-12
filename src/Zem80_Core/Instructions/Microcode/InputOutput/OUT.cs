using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.IO;

namespace Zem80.Core.Instructions
{
    public class OUT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            Registers r = cpu.Registers;

            void @out(byte portNumber, ByteRegister dataRegister, bool bc)
            {
                Port port = cpu.Ports[portNumber];
                byte output = 0;
                if (dataRegister != ByteRegister.None) output = r[dataRegister];
                port.SignalWrite();
                port.WriteByte(output, bc);
            }

            if (instruction.Prefix == 0x00)
            {
                // OUT (n),A
                r.WZ = (ushort)((r.A << 8) + data.Argument1 + 1);
                @out(data.Argument1, ByteRegister.A, false);
            }
            else
            {
                // OUT (C),r
                @out(r.C, instruction.Source.AsByteRegister(), true);
                r.WZ = (ushort)(r.BC + 1);
            }

            return new ExecutionResult(package, flags);
        }

        public OUT()
        {
        }
    }
}

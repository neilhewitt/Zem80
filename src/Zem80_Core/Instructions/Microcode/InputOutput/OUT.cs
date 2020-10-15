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
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            void @out(byte portNumber, ByteRegister dataRegister, bool bc)
            {
                Port port = cpu.Ports[portNumber];
                byte output = r[dataRegister];
                port.SignalWrite();
                port.WriteByte(output, bc);
            }

            if (instruction.Prefix == InstructionPrefix.Unprefixed)
            {
                // OUT (n),A
                @out(data.Argument1, ByteRegister.A, false);
            }
            else
            {
                // OUT (C),r
                @out(r.C, instruction.Source.AsByteRegister(), true);
            }

            return new ExecutionResult(package, flags);
        }

        public OUT()
        {
        }
    }
}

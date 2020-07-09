using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class OUT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            void @out(byte portNumber, ByteRegister dataRegister)
            {
                Port port = cpu.Ports[portNumber];
                byte output = r[dataRegister];
                port.SignalWrite();
                port.WriteByte(output);
            }

            if (instruction.Prefix == InstructionPrefix.Unprefixed)
            {
                // OUT (n),A
                @out(data.Argument1, ByteRegister.A);
            }
            else
            {
                // OUT (C),r
                @out(r.C, instruction.Source.AsByteRegister());
            }

            return new ExecutionResult(package, flags);
        }

        public OUT()
        {
        }
    }
}

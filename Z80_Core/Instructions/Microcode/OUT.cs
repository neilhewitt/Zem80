using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class OUT : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IFlags flags = new Flags();
            IRegisters r = cpu.Registers;

            void @out(byte portNumber, RegisterIndex addressRegister, RegisterIndex dataRegister)
            {
                IPort port = cpu.Ports[portNumber];
                byte output = r[dataRegister];
                cpu.SetAddressBus(portNumber, r[addressRegister]);
                port.SignalWrite();
                cpu.SetDataBus(output);
                port.WriteByte(output);
            }

            if (instruction.Prefix == InstructionPrefix.Unprefixed)
            {
                @out(data.Argument1, RegisterIndex.A, RegisterIndex.A);
            }
            else
            {
                @out(r.C, RegisterIndex.B, data.RegisterIndex.Value);
            }

            return new ExecutionResult(flags, 0);
        }

        public OUT()
        {
        }
    }
}

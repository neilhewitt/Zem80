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
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;

            void @out(byte portNumber, RegisterName addressRegister, RegisterName dataRegister)
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
                @out(data.Argument1, RegisterName.A, RegisterName.A);
            }
            else
            {
                @out(r.C, RegisterName.B, data.Register.Value);
            }

            return new ExecutionResult(package, flags, false);
        }

        public OUT()
        {
        }
    }
}

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

            void @out(byte portNumber, Register addressRegister, Register dataRegister)
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
                @out(data.Argument1, Register.A, Register.A);
            }
            else
            {
                @out(r.C, Register.B, data.Register.Value);
            }

            return new ExecutionResult(package, flags, false);
        }

        public OUT()
        {
        }
    }
}

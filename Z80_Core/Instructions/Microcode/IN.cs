using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class IN : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = new Flags();
            IRegisters r = cpu.Registers;

            byte @in(byte portNumber, RegisterName fromRegister, RegisterName toRegister)
            {
                IPort port = cpu.Ports[portNumber];
                cpu.SetAddressBus(portNumber, r[fromRegister]);
                port.SignalRead();
                byte input = port.ReadByte();
                r[toRegister] = input;
                return input;
            }

            if (instruction.Prefix == InstructionPrefix.Unprefixed)
            {
                // IN A,(n)
                @in(data.Argument1, RegisterName.A, RegisterName.A);
            }
            else
            {
                // IN r,(C)
                byte input = @in(r.C, RegisterName.B, instruction.OperandRegister);
                if ((sbyte)input < 0) flags.Sign = true;
                if (input == 0) flags.Zero = true;
                if (input.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
            }

            return new ExecutionResult(package, flags, false);
        }

        public IN()
        {
        }
    }
}

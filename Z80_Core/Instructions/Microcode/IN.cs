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
            IFlags flags = new Flags();
            IRegisters r = cpu.Registers;

            byte @in(byte portNumber, RegisterIndex fromRegister, RegisterIndex toRegister)
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
                @in(data.Argument1, RegisterIndex.A, RegisterIndex.A);
            }
            else
            {
                byte input = @in(r.C, RegisterIndex.B, data.RegisterIndex.Value);
                if ((sbyte)input < 0) flags.Sign = true;
                if (input == 0) flags.Zero = true;
                if (input.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
            }

            return new ExecutionResult(flags, 0);
        }

        public IN()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class IN : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;
            Registers r = cpu.Registers;

            byte @in(byte portNumber, ByteRegister toRegister)
            {
                Port port = cpu.Ports[portNumber];
                port.SignalRead();
                byte input = port.ReadByte();
                r[toRegister] = input;
                return input;
            }

            if (instruction.Prefix == InstructionPrefix.Unprefixed)
            {
                // IN A,(n)
                @in(data.Argument1, ByteRegister.A);
            }
            else
            {
                // IN r,(C)
                byte input = @in(r.C, instruction.Target.AsByteRegister());
                flags.Sign = ((sbyte)input < 0);
                flags.Zero = (input == 0);
                flags.ParityOverflow = input.EvenParity();
                flags.HalfCarry = false;
                flags.Subtract = false;
                flags.X = (input & 0x08) > 0; // copy bit 3
                flags.Y = (input & 0x20) > 0; // copy bit 5
            }

            return new ExecutionResult(package, flags);
        }

        public IN()
        {
        }
    }
}

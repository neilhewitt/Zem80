using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.IO;

namespace Zem80.Core.Instructions
{
    public class IN : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags;
            Registers r = cpu.Registers;

            byte @in(byte portNumber, ByteRegister toRegister, bool bc)
            {
                Port port = cpu.Ports[portNumber];
                port.SignalRead();
                byte input = port.ReadByte(bc);
                if (toRegister != ByteRegister.F) r[toRegister] = input;
                return input;
            }

            if (instruction.Prefix == 0x00)
            {
                // IN A,(n)
                r.WZ = (ushort)((r.A << 8) + data.Argument1 + 1);
                @in(data.Argument1, ByteRegister.A, false);
            }
            else
            {
                // IN r,(C)
                byte input = @in(r.C, instruction.Target.AsByteRegister(), true);
                flags.Sign = ((sbyte)input < 0);
                flags.Zero = (input == 0);
                flags.ParityOverflow = input.EvenParity();
                flags.HalfCarry = false;
                flags.Subtract = false;
                flags.X = (input & 0x08) > 0; // copy bit 3
                flags.Y = (input & 0x20) > 0; // copy bit 5
                r.WZ = (ushort)(r.BC + 1);
            }

            return new ExecutionResult(package, flags);
        }

        public IN()
        {
        }
    }
}

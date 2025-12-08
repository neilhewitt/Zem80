using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.InputOutput;

namespace Zem80.Core.CPU
{
    public class IN : IN_OUT { public IN() { } }
    public class OUT : IN_OUT { public OUT() { } }

    public class IN_OUT : MicrocodeBase
    {
        // IN/OUT A,(n)
        // IN/OUT r,(C)

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            if (instruction.Prefix == 0x00)
            {
                // IN A,(n)
                r.WZ = (ushort)((r.A << 8) + data.Argument1 + 1);
                if (this is IN)
                {
                    @in(data.Argument1, ByteRegister.A, false);
                }
                else
                {
                    @out(data.Argument1, ByteRegister.A, false);
                }
            }
            else
            {
                if (this is IN)
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
                }
                else
                {
                    @out(r.C, instruction.Source.AsByteRegister(), true);
                }

                r.WZ = (ushort)(r.BC + 1);
            }

            byte @in(byte portNumber, ByteRegister toRegister, bool bc)
            {
                IPort port = cpu.Ports[portNumber];
                port.SignalRead();
                byte input = port.ReadByte(bc);
                if (toRegister != ByteRegister.F) r[toRegister] = input;
                return input;
            }

            void @out(byte portNumber, ByteRegister dataRegister, bool bc)
            {
                IPort port = cpu.Ports[portNumber];
                byte output = 0;
                if (dataRegister != ByteRegister.None) output = r[dataRegister];
                port.SignalWrite();
                port.WriteByte(output, bc);
            }

            return new ExecutionResult(package, flags);
        }

        public IN_OUT()
        {
        }
    }
}

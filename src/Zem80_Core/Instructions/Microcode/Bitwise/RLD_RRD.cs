using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RLD : RLD_RRD { public RLD() : base("RLD") { } }
    public class RRD : RLD_RRD { public RRD() : base("RRD") { } }

    public class RLD_RRD : MicrocodeBase
    {
        bool _isRLD;

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Flags flags = cpu.Flags.Clone();

            byte xHL = cpu.Memory.ReadByteAt(cpu.Registers.HL, (byte?)(_isRLD ? 3 : 4));
            byte A = cpu.Registers.A;

            byte lowA = A.GetLowNybble();
            byte lowxHL = xHL.GetLowNybble();
            byte highxHL = xHL.GetHighNybble();

            // pattern is:
            // RLD: A.low = xHL.high; xHL.high = xHL.low; xHL.low = A.low
            // RRD: A.low = xHL.low;  xHL.high = A.low;   xHL.low = xHL.high;

            A = A.SetLowNybble(_isRLD ? highxHL : lowxHL);
            xHL = xHL.SetHighNybble(_isRLD ? lowxHL : lowA);
            xHL = xHL.SetLowNybble(_isRLD ? lowA : highxHL);

            cpu.Timing.InternalOperationCycle(4);
            cpu.Memory.WriteByteAt(cpu.Registers.HL, xHL, 3);
            cpu.Registers.A = A;

            // bitwise flag lookup doesn't work for this instruction
            flags.Sign = ((sbyte)A < 0);
            flags.Zero = (A == 0);
            flags.ParityOverflow = A.EvenParity();
            flags.HalfCarry = false;
            flags.Subtract = false;
            flags.X = (A & 0x08) > 0; // copy bit 3
            flags.Y = (A & 0x20) > 0; // copy bit 5

            // leave carry alone

            cpu.Registers.WZ = (ushort)(cpu.Registers.HL + 1);

            return new ExecutionResult(package, flags);
        }

        public RLD_RRD(string z80Mnemonic)
        {
            _isRLD = (z80Mnemonic == "RLD");
        }
    }
}

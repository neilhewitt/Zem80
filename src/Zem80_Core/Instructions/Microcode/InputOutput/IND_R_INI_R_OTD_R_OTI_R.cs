using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.InputOutput;

namespace Zem80.Core.CPU
{
    public class INI : IND_R_INI_R_OTD_R_OTI_R { public INI() : base("INI") { } }
    public class IND : IND_R_INI_R_OTD_R_OTI_R { public IND() : base("IND") { } }
    public class INIR : IND_R_INI_R_OTD_R_OTI_R { public INIR() : base("INIR") { } }
    public class INDR : IND_R_INI_R_OTD_R_OTI_R { public INDR() : base("INDR") { } }

    public class OUTI : IND_R_INI_R_OTD_R_OTI_R { public OUTI() : base("OUTI") { } }
    public class OUTD : IND_R_INI_R_OTD_R_OTI_R { public OUTD() : base("OUTD") { } }
    public class OTIR : IND_R_INI_R_OTD_R_OTI_R { public OTIR() : base("OTIR") { } }
    public class OTDR : IND_R_INI_R_OTD_R_OTI_R { public OTDR() : base("OTDR") { } }

    public class IND_R_INI_R_OTD_R_OTI_R : MicrocodeBase
    {
        private bool _increments;
        private bool _repeats;
        private bool _in;

        // this class implements all the block input/output instructions:
        // INI, IND, INIR, INDR, OUTI, OUTD, OTIR, OTDR

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            IPort port = cpu.Ports[r.C];
            int add = _increments ? 1 : -1;
            if (_in)
            {
                port.SignalRead();
                byte input = port.ReadByte(true);
                cpu.Memory.WriteByteAt(r.HL, input, 3);
                if (_repeats) cpu.Timing.InternalOperationCycle(5);
                r.HL = (ushort)(r.HL + add);
                r.WZ = r.BC;
                r.B--;

                flags.Zero = _repeats ? true : (r.B == 0);
                flags.Subtract = true;
                flags.X = (input & 0x08) > 0; // copy bit 3
                flags.Y = (input & 0x20) > 0; // copy bit 5
            }
            else
            {
                byte output = cpu.Memory.ReadByteAt(r.HL, 3);
                r.WZ = r.BC;
                r.B--;
                port.SignalWrite();
                port.WriteByte(output, true);
                r.HL = (ushort)(r.HL + add);

                flags.Zero = _repeats ? true : (r.B == 0);
                flags.Subtract = true;
                flags.X = (output & 0x08) > 0; // copy bit 3
                flags.Y = (output & 0x20) > 0; // copy bit 5

            }

            if (_repeats && r.B != 0) r.PC = package.InstructionAddress;

            return new ExecutionResult(package, flags);
        }

        public IND_R_INI_R_OTD_R_OTI_R(string z80Mnemonic)
        {
            _increments = z80Mnemonic.Contains("NI") || z80Mnemonic.Contains("TI");
            _repeats = z80Mnemonic.EndsWith("R");
            _in = z80Mnemonic.StartsWith("IN");
        }
    }
}

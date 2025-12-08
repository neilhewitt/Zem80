using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class LDI : LDI_LDD_LDIR_LDDR { public LDI() : base("LDI") { } }
    public class LDD : LDI_LDD_LDIR_LDDR { public LDD() : base("LDD") { } }
    public class LDIR : LDI_LDD_LDIR_LDDR { public LDIR() : base("LDIR") { } }
    public class LDDR : LDI_LDD_LDIR_LDDR { public LDDR() : base("LDDR") { } }

    public class LDI_LDD_LDIR_LDDR : MicrocodeBase
    {
        bool _increments;
        bool _repeats;

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            int add = _increments ? 1 : -1;
            byte value = cpu.Memory.ReadByteAt(r.HL, 3);
            cpu.Memory.WriteByteAt(r.DE, value, 5);
            r.HL = (ushort)(r.HL + add);
            r.DE = (ushort)(r.DE + add);
            r.BC--;

            flags.HalfCarry = false;
            flags.ParityOverflow = (r.BC != 0);
            flags.Subtract = false;
            flags.X = (((byte)(value + cpu.Registers.A)) & 0x08) > 0; // copy bit 3
            flags.Y = (((byte)(value + cpu.Registers.A)) & 0x02) > 0; // copy bit 1 (note: non-standard behaviour)

            if (_repeats)
            {
                bool conditionTrue = (r.BC == 0);
                if (conditionTrue)
                {
                    cpu.Timing.InternalOperationCycle(5);
                    r.WZ = (ushort)(r.PC + 1);
                }
                else
                {
                    r.PC = package.InstructionAddress; // repeat the instruction until BC is zero
                }
            }

            return new ExecutionResult(package, flags);
        }

        public LDI_LDD_LDIR_LDDR(string z80Mnemonic)
        {
            _increments = (z80Mnemonic == "LDI" || z80Mnemonic == "LDIR");
            _repeats = (z80Mnemonic == "LDIR" || z80Mnemonic == "LDDR");
        }
    }
}

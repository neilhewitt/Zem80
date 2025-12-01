using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class CPDR : CPXX, IMicrocode { public CPDR() : base("CPDR") {} }
    public class CPIR : CPXX, IMicrocode { public CPIR() : base("CPIR") {} }
    public class CPD : CPXX, IMicrocode { public CPD() : base("CPD") {} }
    public class CPI : CPXX, IMicrocode { public CPI() : base("CPI") {} }

    public class CPXX : IMicrocode
    {
        // this class supplies the microcode for CPI, CPD, CPIR and CPDR
        // which are all substantially the same except for the increment/decrement
        // and repeat/no-repeat behaviour

        bool _increments;
        bool _repeats;

        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            bool carry = cpu.Flags.Carry; // current value before CPD
            byte a = cpu.Registers.A;
            byte b = cpu.Memory.ReadByteAt(cpu.Registers.HL, 3);

            (byte result, Flags flags) = Arithmetic.Subtract(a, b, false);

            int add = _increments ? 1 : -1; // CPD vs CPI

            cpu.Registers.BC--;
            flags.ParityOverflow = (cpu.Registers.BC != 0);
            cpu.Registers.HL = (ushort)(cpu.Registers.HL + add);

            flags.Subtract = true;
            flags.Carry = carry;
            byte valueXY = (byte)(a - b - (flags.HalfCarry ? 1 : 0));
            flags.X = (valueXY & 0x08) > 0; // copy bit 3
            flags.Y = (valueXY & 0x02) > 0; // copy bit 1 (note: non-standard behaviour)

            cpu.Registers.WZ = (ushort)(cpu.Registers.WZ + add);

            if (_repeats)
            {
                if (flags.Zero || !flags.ParityOverflow)
                {
                    cpu.Timing.InternalOperationCycle(5);
                }
                else
                {
                    cpu.Registers.PC = package.InstructionAddress;
                    cpu.Registers.WZ = (ushort)(cpu.Registers.PC + 1);
                }
            }
            else
            {
                cpu.Timing.InternalOperationCycle(5);
            }

            return new ExecutionResult(package, flags);
        }

        public CPXX(string z80Mnemonic)
        {
            _increments = z80Mnemonic == "CPI" || z80Mnemonic == "CPIR";
            _repeats = z80Mnemonic == "CPIR" || z80Mnemonic == "CPDR";
        }
    }
}

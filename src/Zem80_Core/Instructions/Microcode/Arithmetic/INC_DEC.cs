using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class INC : INC_DEC { public INC() : base("INC") { } }
    public class DEC : INC_DEC { public DEC() : base("DEC") { } }

    public class INC_DEC : MicrocodeBase
    {
        // INC/DEC r
        // INC/DEC (HL)
        // INC/DEC (IX+o)
        // INC/DEC (IY+o)
        // INC/DEC rr

        private bool _inc;

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            Flags flags = cpu.Flags.Clone();

            if (instruction.TargetsWordRegister)
            {
                // inc 16-bit
                WordRegister register = instruction.Target.AsWordRegister();
                ushort value = r[register];
                r[register] = (ushort)(_inc ? value + 1 : value - 1);
            }
            else
            {
                byte value = 0;
                if (instruction.TargetsByteInMemory)
                {
                    // inc byte in memory
                    if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                    value = Resolver.GetSourceByte(instruction, data, cpu, out ushort address, 4);
                    cpu.Memory.WriteByteAt(address, (byte)(_inc ? value + 1 : value - 1), 3);
                }
                else
                {
                    // it's an 8-bit inc
                    ByteRegister register = instruction.Target.AsByteRegister();
                    value = r[register];
                    r[register] = (byte)(_inc ? value + 1 : value - 1);
                }

                int result = _inc ? value + 1 : value - 1;
                bool carry = flags.Carry;
                flags.Zero = ((byte)result == 0);
                flags.Sign = ((sbyte)result < 0);
                flags.HalfCarry = ((value ^ (byte)(result & 0xFF) ^ 1) & 0x10) != 0;
                flags.ParityOverflow = (value == (_inc ? 0x7F : 0x80));
                flags.Subtract = !_inc;
                flags.X = (result & 0x08) > 0; // copy bit 3
                flags.Y = (result & 0x20) > 0; // copy bit 5
                flags.Carry = carry; // always unaffected
            }

            return new ExecutionResult(package, flags);
        }

        public INC_DEC(string z80Mnemonic)
        {
            _inc = z80Mnemonic == ("INC");
        }
    }
}

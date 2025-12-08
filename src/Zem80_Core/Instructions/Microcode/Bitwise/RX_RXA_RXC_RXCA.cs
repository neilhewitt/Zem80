using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RL : RX_RXA_RXC_RXCA { public RL() : base("RL") { } }
    public class RLA : RX_RXA_RXC_RXCA { public RLA() : base("RLA") { } }
    public class RLC : RX_RXA_RXC_RXCA { public RLC() : base("RLC") { } }
    public class RLCA : RX_RXA_RXC_RXCA { public RLCA() : base("RLCA") { } }
    public class RR : RX_RXA_RXC_RXCA { public RR() : base("RR") { } }
    public class RRA : RX_RXA_RXC_RXCA { public RRA() : base("RRA") { } }
    public class RRC : RX_RXA_RXC_RXCA { public RRC() : base("RRC") { } }
    public class RRCA : RX_RXA_RXC_RXCA { public RRCA() : base("RRCA") { } }

    public class RX_RXA_RXC_RXCA : MicrocodeBase
    {
        private bool _left;
        private bool _throughA;
        private bool _withCarry;
        private bool _bcd;
        private string _mnemonic;

        // this class implements all of the bitwise rotation instructions (except RLD/RRD) as a complex:
        // RL, RLA, RLC, RLCA, RR, RRA, RRC, RRCA

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;

            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.Target.AsByteRegister();
            bool previousCarry = flags.Carry;

            // for RL[C]A and RR[C]A, we need to clear specific flags
            FlagState? flagsToSet = null;
            if (this is RLA || this is RRA || this is RLCA || this is RRCA)
            {
                flagsToSet = (FlagState.Carry | FlagState.HalfCarry | FlagState.Subtract | FlagState.X | FlagState.Y);
            }

            byte original, shifted;
            if (register != ByteRegister.None)
            {
                original = r[register];
                (shifted, flags) = (_mnemonic) switch
                {
                    "RL" => Bitwise.RotateLeftThroughCarry(original, flags),
                    "RLC" => Bitwise.RotateLeft(original, flags),
                    "RLA" => Bitwise.RotateLeftThroughCarry(original, flags, flagsToSet),
                    "RLCA" => Bitwise.RotateLeft(original, flags, flagsToSet),
                    "RR" => Bitwise.RotateRightThroughCarry(original, flags),
                    "RRC" => Bitwise.RotateRight(original, flags),
                    "RRA" => Bitwise.RotateRightThroughCarry(original, flags, flagsToSet),
                    "RRCA" => Bitwise.RotateRight(original, flags, flagsToSet),
                    _ => throw new NotImplementedException("Rotation type not implemented.")
                };

                r[register] = shifted;
            }
            else
            {
                // RL/RR or RLC/RRC only

                ushort address = Resolver.GetSourceAddress(instruction, cpu, offset);
                original = cpu.Memory.ReadByteAt(address, 4);
                (shifted, flags) = (_mnemonic) switch
                {
                    "RL" => Bitwise.RotateLeftThroughCarry(original, flags),
                    "RLC" => Bitwise.RotateLeft(original, flags),
                    "RR" => Bitwise.RotateRightThroughCarry(original, flags),
                    "RRC" => Bitwise.RotateRight(original, flags),
                    _ => throw new NotImplementedException("Rotation type not implemented.")
                };

                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(4);
                cpu.Memory.WriteByteAt(address, shifted, 3);

                if (instruction.CopiesResultToRegister)
                {
                    r[instruction.CopyResultTo] = shifted;
                }
            }

            return new ExecutionResult(package, flags);
        }

        public RX_RXA_RXC_RXCA(string z80Mnemonic)
        {
            _left = z80Mnemonic.StartsWith("RL");
            _throughA = z80Mnemonic.EndsWith("A");
            _withCarry = z80Mnemonic.Contains("C");
            _bcd = z80Mnemonic.Contains("D");
            _mnemonic = z80Mnemonic;
        }
    }
}

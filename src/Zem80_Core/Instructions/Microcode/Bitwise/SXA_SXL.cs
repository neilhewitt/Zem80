using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class SLA : SXA_SXL { public SLA() : base("SLA") { } }
    public class SLL : SXA_SXL { public SLL() : base("SLL") { } }
    public class SRA : SXA_SXL { public SRA() : base("SRA") { } }
    public class SRL : SXA_SXL { public SRL() : base("SRL") { } }

    public class SXA_SXL : MicrocodeBase
    {
        private string _mnemonic;

        // this class implements all of the bitwise shift instructions as a complex:
        // SLA, SLL, SRA, SRL

        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Flags.Clone();
            IRegisters r = cpu.Registers;
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.Target.AsByteRegister();

            byte original, shifted;
            if (register != ByteRegister.None)
            {
                original = r[register];

                (shifted, flags) = _mnemonic switch
                {
                    "SLA" => Bitwise.ShiftLeftResetBit0(original, flags),
                    "SLL" => Bitwise.ShiftLeftSetBit0(original, flags),
                    "SRA" => Bitwise.ShiftRightPreserveBit7(original, flags),
                    "SRL" => Bitwise.ShiftRightResetBit7(original, flags),

                    _ => throw new NotSupportedException($"Mnemonic {_mnemonic} is not supported in SXA_SXL microcode."),
                };

                r[register] = shifted;
            }
            else
            {
                ushort address = Resolver.GetSourceAddress(instruction, cpu, offset);
                original = cpu.Memory.ReadByteAt(address, 4);
                (shifted, flags) = (shifted, flags) = _mnemonic switch
                {
                    "SLA" => Bitwise.ShiftLeftResetBit0(original, flags),
                    "SLL" => Bitwise.ShiftLeftSetBit0(original, flags),
                    "SRA" => Bitwise.ShiftRightPreserveBit7(original, flags),
                    "SRL" => Bitwise.ShiftRightResetBit7(original, flags),

                    _ => throw new NotSupportedException($"Mnemonic {_mnemonic} is not supported in SXA_SXL microcode."),
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

        public SXA_SXL(string z80Mnemonic)
        {
            _mnemonic = z80Mnemonic;
        }
    }
}

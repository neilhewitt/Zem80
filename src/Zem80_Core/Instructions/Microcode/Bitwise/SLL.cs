using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class SLL : MicrocodeBase
    {
        // SLL r
        // SLL (HL)
        // SLL (IX+o)
        // SLL (IY+o)

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
                (shifted, flags) = Bitwise.ShiftLeftSetBit0(original, flags);
                r[register] = shifted;
            }
            else
            {
                ushort address = Resolver.GetSourceAddress(instruction, cpu, offset);
                original = cpu.Memory.ReadByteAt(address, 4);
                (shifted, flags) = Bitwise.ShiftLeftSetBit0(original, flags);
                cpu.Memory.WriteByteAt(address, shifted, 3);
                if (instruction.CopiesResultToRegister)
                {
                    r[instruction.CopyResultTo] = shifted;
                }
            }

            return new ExecutionResult(package, flags);
        }

        public SLL()
        {
        }
    }
}

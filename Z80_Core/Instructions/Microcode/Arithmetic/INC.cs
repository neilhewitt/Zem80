using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Text;

namespace Z80.Core
{
    public class INC : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            byte offset = data.Argument1;
            Flags flags = cpu.Registers.Flags;

            if (instruction.TargetsWordRegister)
            {
                // inc 16-bit
                WordRegister register = instruction.Target.AsWordRegister();
                ushort value = r[register];
                r[register] = (ushort)(value + 1);
            }
            else
            {
                byte value = 0;
                if (instruction.TargetsByteInMemory)
                {
                    // inc byte in memory
                    if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                    value = instruction.MarshalSourceByte(data, cpu, out ushort address, out ByteRegister source);
                    cpu.Memory.WriteByteAt(address, (byte)(value + 1), false);
                }
                else
                {
                    // it's an 8-bit inc
                    ByteRegister register = instruction.Target.AsByteRegister();
                    value = r[register];
                    r[register] = (byte)(value + 1);
                }

                bool carry = flags.Carry;
                flags = FlagLookup.ByteArithmeticFlags(value, 1, false, false);
                flags.ParityOverflow = (value == 0x7F);
                flags.Carry = carry; // always unaffected
                flags.Subtract = false;
            }

            return new ExecutionResult(package, flags);
        }

        public INC()
        {
        }
    }
}

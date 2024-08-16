using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class DEC : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            Flags flags = cpu.Flags.Clone();

            if (instruction.TargetsWordRegister)
            {
                //dec 16 - bit
                WordRegister register = instruction.Target.AsWordRegister();
                ushort value = r[register];
                r[register] = (ushort)(value - 1);
            }
            else
            {
                byte value = 0;
                if (instruction.TargetsByteInMemory)
                {
                    // dec byte in memory
                    if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);
                    value = instruction.MarshalSourceByte(data, cpu, out ushort address, 4);
                    cpu.Memory.WriteByteAt(address, (byte)(value - 1), 3);
                }
                else
                {
                    // it's an 8-bit dec
                    ByteRegister register = instruction.Target.AsByteRegister();
                    value = r[register];
                    r[register] = (byte)(value - 1);
                }

                bool carry = flags.Carry;
                flags = FlagLookup.ByteArithmeticFlags(value, 1, false, true);
                flags.ParityOverflow = (value == 0x80);
                flags.Carry = carry; // always unaffected
            }

            return new ExecutionResult(package, flags);
        }

        public DEC()
        {
        }
    }
}

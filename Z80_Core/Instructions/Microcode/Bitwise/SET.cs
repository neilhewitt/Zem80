using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SET : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            byte bitIndex = instruction.GetBitIndex();
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.GetByteRegister();

            if (register != ByteRegister.None)
            {
                byte value = r[register].SetBit(bitIndex, true);
                r[register] = value;
            }
            else
            {
                ushort address = instruction.Prefix switch
                {
                    InstructionPrefix.CB => r.HL,
                    InstructionPrefix.DDCB => (ushort)(r.IX + offset),
                    InstructionPrefix.FDCB => (ushort)(r.IY + offset),
                    _ => (ushort)0xFFFF
                };
                if (instruction.HLIX || instruction.HLIY) cpu.Timing.InternalOperationCycle(5);
                byte value = cpu.Memory.ReadByteAt(address, false);
                value = value.SetBit(bitIndex, true);
                cpu.Memory.WriteByteAt(address, value, false);
            }

            return new ExecutionResult(package, null, false, false);
        }

        public SET()
        {
        }
    }
}

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
            byte bitIndex = instruction.BitIndex ?? 0xFF;
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.OperandRegister;

            if (register != ByteRegister.None)
            {
                r[register] = r[register].SetBit(bitIndex, true);
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
                if (instruction.HLIX || instruction.HLIY) cpu.InternalOperationCycle(5);
                byte value = cpu.Memory.ReadByteAt(address);
                value = value.SetBit(bitIndex, true);
                cpu.Memory.WriteByteAt(address, value);
            }

            return new ExecutionResult(package, cpu.Registers.Flags, false, false);
        }

        public SET()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RES : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            byte bitIndex = instruction.BitIndex ?? 0xFF;
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.OperandByteRegister;

            if (register != ByteRegister.None)
            {
                r[register] = r[register].SetBit(bitIndex, false);
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
                cpu.Memory.WriteByteAt(address, cpu.Memory.ReadByteAt(address).SetBit(bitIndex, false));
            }

            return new ExecutionResult(package, cpu.Registers.Flags, false, false);
        }

        public RES()
        {
        }
    }
}

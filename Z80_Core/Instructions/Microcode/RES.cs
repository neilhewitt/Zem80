using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RES : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            byte bitIndex = data.BitIndex ?? 0xFF;
            sbyte offset = (sbyte)(data.Argument1);
            RegisterIndex index = data.RegisterIndex ?? RegisterIndex.None;

            if (index != RegisterIndex.None)
            {
                r[index] = r[index].SetBit(bitIndex, false);
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

            return new ExecutionResult(package, new Flags(), false);
        }

        public RES()
        {
        }
    }
}
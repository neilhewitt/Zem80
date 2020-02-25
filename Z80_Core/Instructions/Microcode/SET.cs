using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SET : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            IRegisters r = cpu.Registers;
            byte bitIndex = data.BitIndex ?? 0xFF;
            sbyte offset = (sbyte)(data.Argument1);
            RegisterName register = data.Register ?? RegisterName.None;

            if (register != RegisterName.None)
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
                cpu.Memory.WriteByteAt(address, cpu.Memory.ReadByteAt(address).SetBit(bitIndex, true));
            }

            return new ExecutionResult(package, cpu.Registers.Flags, false);
        }

        public SET()
        {
        }
    }
}

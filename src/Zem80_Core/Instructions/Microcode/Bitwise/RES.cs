using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class RES : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            byte bitIndex = instruction.GetBitIndex();
            sbyte offset = (sbyte)(data.Argument1);
            ByteRegister register = instruction.Source.AsByteRegister();

            if (register != ByteRegister.None)
            {
                byte value = r[register].SetBit(bitIndex, false);
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
                if (instruction.IsIndexed) cpu.Timing.InternalOperationCycle(5);

                byte value = cpu.Memory.Timed.ReadByteAt(address);
                value = value.SetBit(bitIndex, false);
                cpu.Memory.Timed.WriteByteAt(address, value);
                if (instruction.CopyResultTo != ByteRegister.None)
                {
                    r[instruction.CopyResultTo.Value] = value;
                }
            }

            return new ExecutionResult(package, null);
        }

        public RES()
        {
        }
    }
}

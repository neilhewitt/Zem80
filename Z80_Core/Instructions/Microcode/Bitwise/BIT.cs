using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class BIT : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Registers r = cpu.Registers;
            Flags flags = cpu.Registers.Flags;
            InstructionPrefix prefix = instruction.Prefix;

            byte bitIndex = instruction.BitIndex.Value;
            byte value;
            if (instruction.OperandRegister != ByteRegister.None)
            {
                value = r[instruction.OperandRegister]; // BIT b, r
            }
            else
            {
                ushort address = instruction.HLIX ? r.IX : instruction.HLIY ? r.IY : r.HL; // BIT b, (HL / IX+o / IY+o)
                sbyte offset = (instruction.HLIY || instruction.HLIY) ? (sbyte)data.Argument1 : (sbyte)0;
                if (instruction.HLIX || instruction.HLIY) cpu.InternalOperationCycle(5);
                value = cpu.Memory.ReadByteAt((ushort)(address + offset));
            }
           
            flags.Zero = value.GetBit(bitIndex) == false;
            flags.HalfCarry = true;
            flags.Subtract = false;

            return new ExecutionResult(package, flags, false, false);
        }

        public BIT()
        {
        }
    }
}

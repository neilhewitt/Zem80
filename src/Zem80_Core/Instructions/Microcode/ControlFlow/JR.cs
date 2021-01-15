using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class JR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            if (instruction.Condition == Condition.None || cpu.Registers.Flags.SatisfyCondition(instruction.Condition))
            {
                cpu.Timing.InternalOperationCycle(5);
                ushort address = (ushort)(cpu.Registers.PC - 2); // wind back to the address of the JR instruction as PC has already moved on

                // the jump is relative to the address of the JR instruction but the jump *displacement* is calculated from the start of the *next* instruction. 
                // This means the actual jump range is -126 to +129 bytes, *not* 127 bytes each way. Z80 assemblers compensate for this by 
                // adjusting the jump value, so for example 'JR 0' would actually end up being 'JR 2' and would set PC to the start of the next
                // instruction - hence we must add two bytes here to resolve the correct target address
                address = (ushort)(address + (sbyte)data.Argument1 + 2);

                cpu.Registers.PC = address;
                cpu.Registers.Flags.X = (address & 0x08) > 0; // copy bit 3
                cpu.Registers.Flags.Y = (address & 0x20) > 0; // copy bit 5

                cpu.Registers.WZ = address;
            }

            return new ExecutionResult(package, cpu.Registers.Flags);
        }

        public JR()
        {
        }
    }
}

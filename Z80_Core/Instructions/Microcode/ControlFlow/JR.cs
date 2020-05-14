using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class JR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            bool conditionTrue = false;
            bool jumpRequired = false;

            void jr()
            {
                cpu.Timing.InternalOperationCycle(5);
                ushort address = (ushort)(cpu.Registers.PC - 2); // wind back to the address of the JR instruction as PC has already moved on

                // the jump is relative to the address of the JR instruction but the jump *displacement* is calculated from the start of the *next* instruction. 
                // This means the actual jump range is -126 to +129 bytes, *not* 127 bytes each way. Z80 assemblers compensate for this by 
                // adjusting the jump value, so for example 'JR 0' would actually end up being 'JR 2' and would set PC to the start of the next
                // instruction - hence we must add two bytes here to resolve the correct target address
                address = (ushort)(address + (sbyte)data.Argument1 + 2);

                cpu.Registers.PC = (address);
                conditionTrue = instruction.Opcode != 0x18;
                jumpRequired = true;
            }

            switch (instruction.Prefix)
            {
                case InstructionPrefix.Unprefixed:
                    switch (instruction.Opcode)
                    {
                        case 0x18: // JR o
                            jr();
                            break;
                        case 0x20: // JR NZ,o
                            if (!cpu.Registers.Flags.Zero) jr();
                            break;
                        case 0x28: // JR Z,o
                            if (cpu.Registers.Flags.Zero) jr();
                            break;
                        case 0x30: // JR NC,o
                            if (!cpu.Registers.Flags.Carry) jr();
                            break;
                        case 0x38: // JR C,o
                            if (cpu.Registers.Flags.Carry) jr();
                            break;
                    }
                    break;
            }

            return new ExecutionResult(package, cpu.Registers.Flags, conditionTrue, jumpRequired);
        }

        public JR()
        {
        }
    }
}

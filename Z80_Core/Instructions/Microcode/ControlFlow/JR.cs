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
            Flags flags = cpu.Registers.Flags;

            void jr()
            {
                cpu.InternalOperationCycle(5);
                cpu.Registers.PC = (ushort)(cpu.Registers.PC + (sbyte)data.Argument1 + instruction.SizeInBytes);
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
                            if (!flags.Zero) jr();
                            break;
                        case 0x28: // JR Z,o
                            if (flags.Zero) jr();
                            break;
                        case 0x30: // JR NC,o
                            if (!flags.Carry) jr();
                            break;
                        case 0x38: // JR C,o
                            if (flags.Carry) jr();
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class JR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            sbyte offset = (sbyte)data.Argument1;
            Flags flags = cpu.Registers.Flags;
            bool pcWasSet = false;

            void jr()
            {
                cpu.Registers.PC += 2; // include this instruction length
                cpu.Registers.PC += (ushort)offset;
                pcWasSet = true;
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

            return new ExecutionResult(package, cpu.Registers.Flags, !pcWasSet, pcWasSet);
        }

        public JR()
        {
        }
    }
}

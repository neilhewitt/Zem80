using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class DJNZ : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            bool pcWasSet = true;

            if (cpu.Registers.B > 0)
            {
                cpu.Registers.PC += 2; // include size of this instruction
                cpu.Registers.B--;
                sbyte jump = (sbyte)data.Argument1; 
                cpu.Registers.PC += (ushort)jump;
                if (jump == 0) pcWasSet = false;
            }
            else
            {
                pcWasSet = false;
            }

            return new ExecutionResult(package, cpu.Registers.Flags, !pcWasSet, pcWasSet);
        }

        public DJNZ()
        {
        }
    }
}

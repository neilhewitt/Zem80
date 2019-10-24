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
                cpu.Registers.B--;
                sbyte jump = (sbyte)(data.Arguments[0]);
                if (jump > 0) cpu.Registers.PC += (ushort)jump;
                if (jump < 0) cpu.Registers.PC -= (ushort)jump;
                if (jump == 0) pcWasSet = false;
            }
            else
            {
                pcWasSet = false;
            }

            return new ExecutionResult(new Flags(), 0, pcWasSet);
        }

        public DJNZ()
        {
        }
    }
}

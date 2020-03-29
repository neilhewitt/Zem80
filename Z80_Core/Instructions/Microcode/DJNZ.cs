using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class DJNZ : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, ExecutionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            bool conditionTrue = false;

            sbyte jump = 0;
            if (cpu.Registers.B > 0)
            {
                conditionTrue = true;
                cpu.Registers.B--;
                jump = (sbyte)data.Argument1;
                cpu.Registers.PC = (ushort)(cpu.Registers.PC + jump);
            }

            return new ExecutionResult(package, cpu.Registers.Flags, conditionTrue, jump != 0);
        }

        public DJNZ()
        {
        }
    }
}

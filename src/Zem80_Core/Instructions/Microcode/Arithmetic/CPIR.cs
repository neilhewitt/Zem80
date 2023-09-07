using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class CPIR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            CPI cpi = new CPI();
            ExecutionResult result = cpi.Execute(cpu, package);

            if (result.Flags.Zero || !result.Flags.ParityOverflow)
            {
                cpu.Timing.InternalOperationCycle(5);
            }
            else
            {
                cpu.Registers.PC = package.InstructionAddress;
                cpu.Registers.WZ = (ushort)(cpu.Registers.PC + 1);
            }

            return new ExecutionResult(package, result.Flags);
        }

        public CPIR()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class DJNZ : MicrocodeBase
    {
        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            InstructionData data = package.Data;

            cpu.Registers.B--;
            if (cpu.Registers.B > 0)
            {
                cpu.Timing.InternalOperationCycle(5);
                ushort address = (ushort)(cpu.Registers.PC - 2); // wind back to the address of the DJNZ instruction as PC has already moved on

                // the jump is relative to the address of the DJNZ instruction but the jump *displacement* is calculated from the start of the *next* instruction. 
                // This means the actual jump range is -126 to +129 bytes, *not* 127 bytes each way. Z80 assemblers compensate for this by 
                // adjusting the jump value, so for example 'DJNZ 0' would actually end up being 'DJNZ 2' and would set PC to the start of the next
                // instruction - hence we must add two bytes here to resolve the correct target address
                address = (ushort)(address + (sbyte)data.Argument1 + 2);
                cpu.Registers.PC = address;

                cpu.Registers.WZ = address;
            }

            return new ExecutionResult(package, null);
        }

        public DJNZ()
        {
        }
    }
}

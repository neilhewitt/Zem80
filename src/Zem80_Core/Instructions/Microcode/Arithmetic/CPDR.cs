using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class CPDR : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;
            Flags flags = cpu.Registers.Flags;

            bool carry = flags.Carry;
            byte a = cpu.Registers.A;
            byte b = cpu.Memory.ReadByteAt(cpu.Registers.HL, false);

            var compare = ALUOperations.Subtract(a, b, false);
            flags = compare.Flags;

            cpu.Registers.BC--;
            flags.ParityOverflow = (cpu.Registers.BC != 0);

            cpu.Registers.HL--;

            flags.Subtract = true;
            flags.Carry = carry;

            byte valueXY = (byte)(a - b - (flags.HalfCarry ? 1 : 0));
            flags.X = (valueXY & 0x08) > 0; // copy bit 3
            flags.Y = (valueXY & 0x02) > 0; // copy bit 1 (note: non-standard behaviour)

            bool conditionTrue = (compare.Result == 0 || cpu.Registers.BC == 0);
            if (conditionTrue)
            {
                cpu.Cycle.InternalOperationCycle(5);
                cpu.Registers.WZ++;
            }
            else
            {
                cpu.Registers.PC = package.InstructionAddress;
                cpu.Registers.WZ = (ushort)(cpu.Registers.PC + 1);
            }

            return new ExecutionResult(package, flags, conditionTrue);
        }

        public CPDR()
        {
        }
    }
}

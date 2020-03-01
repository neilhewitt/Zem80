using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RST : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            byte t_index = instruction.Opcode.GetByteFromBits(3, 3);
            ushort address = (ushort)(t_index * 8);

            cpu.Push(RegisterPairName.PC);
            cpu.Registers.PC = address;

            return new ExecutionResult(package, cpu.Registers.Flags, false, true);
        }

        public RST()
        {
        }
    }
}

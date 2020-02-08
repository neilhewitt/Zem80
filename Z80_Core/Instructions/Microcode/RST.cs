using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RST : IInstructionImplementation
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            byte t_index = instruction.Opcode.GetByteFromBits(3, 3);
            ushort address = t_index switch
            {
                0x00 => 0x0000,
                0x01 => 0x0008,
                0x02 => 0x0010,
                0x03 => 0x0018,
                0x04 => 0x0020,
                0x05 => 0x0028,
                0x06 => 0x0030,
                0x07 => 0x0038,
                   _ => 0x0000
            };

            cpu.Push(RegisterPairIndex.PC);
            cpu.Registers.PC = address;

            return new ExecutionResult(package, cpu.Registers.Flags, false, true);
        }

        public RST()
        {
        }
    }
}

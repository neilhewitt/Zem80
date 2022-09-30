using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class RST : IMicrocode
    {
        public ExecutionResult Execute(Processor cpu, InstructionPackage package)
        {
            Instruction instruction = package.Instruction;
            InstructionData data = package.Data;

            byte t_index = instruction.Opcode.GetByteFromBits(3, 3);
            ushort address = (ushort)(t_index * 8);

            cpu.Stack.Push(WordRegister.PC);
            cpu.Registers.PC = (address);
            cpu.Registers.WZ = cpu.Registers.PC;

            return new ExecutionResult(package, null);
        }

        public RST()
        {
        }
    }
}

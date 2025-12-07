using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class RST : MicrocodeBase
    {
        public override ExecutionResult Execute(Processor cpu, InstructionPackage package, Action<ExecutionState> onMachineCycle)
        {
            Instruction instruction = package.Instruction;

            byte t_index = instruction.LastOpcodeByte.GetByteFromBits(3, 3);
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

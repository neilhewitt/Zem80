using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.CPU
{
    public class InstructionPackage
    {
        public Instruction Instruction { get; init; }
        public InstructionData Data { get; init; }
        public ushort InstructionAddress { get; init; }

        public InstructionPackage(Instruction instruction, InstructionData data, ushort instructionAddress)
        {
            Instruction = instruction;
            Data = data;
            InstructionAddress = instructionAddress;
        }
    }
}

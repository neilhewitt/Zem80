using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;
using Zem80.Core.Instructions;

namespace Zem80.Core.Instructions
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

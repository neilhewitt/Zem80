using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.Instructions;

namespace Zem80.Core
{
    public class ExecutionPackage
    {
        public Instruction Instruction { get; set; }
        public InstructionData Data { get; set; }
        public ushort InstructionAddress { get; set; }

        public ExecutionPackage(Instruction instruction, InstructionData data, ushort instructionAddress)
        {
            Instruction = instruction;
            Data = data;
            InstructionAddress = instructionAddress;
        }
    }
}

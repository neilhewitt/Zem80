using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class InstructionPackage
    {
        public Instruction Instruction { get; }
        public InstructionData Data { get; }

        public InstructionPackage(Instruction instruction, InstructionData data)
        {
            Instruction = instruction;
            Data = data;
        }
    }
}

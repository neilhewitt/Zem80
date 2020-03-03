using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ExecutionPackage
    {
        public Instruction Instruction { get; }
        public InstructionData Data { get; }

        public ExecutionPackage(Instruction instruction, InstructionData data)
        {
            Instruction = instruction;
            Data = data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ExecutionPackage
    {
        public Instruction Instruction { get; set; }
        public InstructionData Data { get; set; }

        public ExecutionPackage(Instruction instruction, InstructionData data)
        {
            Instruction = instruction;
            Data = data;
        }
    }
}

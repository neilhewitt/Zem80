using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ExecutionResult
    {
        public Instruction Instruction { get; }
        public InstructionData Data { get; }
        public Flags Flags { get; }
        public ushort InstructionAddress { get; }

        public ExecutionResult(ExecutionPackage package, Flags flags)
        {
            InstructionAddress = package.InstructionAddress;
            Instruction = package.Instruction;
            Data = package.Data;
            Flags = flags;
        }
    }
}
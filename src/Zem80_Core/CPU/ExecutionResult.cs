using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.Instructions;

namespace Zem80.Core.CPU
{
    public class ExecutionResult
    {
        public Instruction Instruction { get; }
        public InstructionData Data { get; }
        public Flags Flags { get; }
        public ushort InstructionAddress { get; }
        public int WaitCyclesAdded { get; internal set; }

        public ExecutionResult(InstructionPackage package, Flags flags)
        {
            InstructionAddress = package.InstructionAddress;
            Instruction = package.Instruction;
            Data = package.Data;
            Flags = flags;
        }
    }
}
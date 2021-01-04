using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.Instructions;

namespace Zem80.Core
{
    public class ExecutionResult
    {
        public Instruction Instruction { get; }
        public InstructionData Data { get; }
        public Flags Flags { get; }
        public ushort InstructionAddress { get; }
        public bool SkipInterruptAfterExecution { get; }

        public ExecutionResult(InstructionPackage package, Flags flags, bool skipInterruptsAfterExecution = false)
        {
            InstructionAddress = package.InstructionAddress;
            Instruction = package.Instruction;
            Data = package.Data;
            Flags = flags;
            SkipInterruptAfterExecution = skipInterruptsAfterExecution;
        }
    }
}
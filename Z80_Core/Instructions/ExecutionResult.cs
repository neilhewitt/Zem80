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
        public bool ConditionIsTrue { get; }
        public bool InstructionSetsProgramCounter { get; }
        public ushort InstructionAddress { get; }

        public ExecutionResult(ExecutionPackage package, Flags flags, bool conditionTrue, bool instructionSetsProgramCounter)
        {
            InstructionAddress = package.InstructionAddress;
            Instruction = package.Instruction;
            Data = package.Data;
            Flags = flags;
            InstructionSetsProgramCounter = instructionSetsProgramCounter;
            ConditionIsTrue = conditionTrue;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ExecutionResult
    {
        public Instruction Instruction { get; }
        public Flags Flags { get; }
        public byte ClockCycles { get; }
        public bool InstructionSetsProgramCounter { get; }

        public ExecutionResult(ExecutionPackage package, Flags flags, bool conditionTrue, bool instructionSetsProgramCounter)
        {
            Instruction = package.Instruction;
            Flags = flags;
            ClockCycles = (byte)(conditionTrue ?
                package.Instruction.ClockCyclesConditional ?? package.Instruction.ClockCycles :
                package.Instruction.ClockCycles);
            InstructionSetsProgramCounter = instructionSetsProgramCounter;
        }
    }
}
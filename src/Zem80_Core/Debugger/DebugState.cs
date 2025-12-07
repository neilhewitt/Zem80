using System;
using Zem80.Core.CPU;

namespace Zem80.Core.Debugger
{
    public class DebugState
    {
        public ushort Address { get; }
        public IRegisters Registers { get; }
        public IReadOnlyFlags Flags { get; }
        public Instruction Instruction { get; }
        public byte Arg1 { get; }
        public byte Arg2 { get; }
        public bool ProgramCounterWasModified { get; }
        public bool IsAfterInstructionExecution { get; }

        public string EventName { get; }
        public string Disassembly { get; }

        public DebugState(IRegisters registers, ExecutionResult result, string eventName)
        {
            Registers = registers.Snapshot();
            Flags = result.Flags;
            
            Address = result.InstructionAddress;
            Instruction = result.Instruction;
            Arg1 = result.Data.Argument1;
            Arg2 = result.Data.Argument2;
            EventName = eventName;

            Disassembly = Instruction.Disassemble(Instruction, Arg1, Arg2);
            ProgramCounterWasModified = result.ProgramCounterWasModified;
            IsAfterInstructionExecution = true;
        }

        public DebugState(IRegisters registers, IReadOnlyFlags flags, InstructionPackage instructionPackage, string eventName)
        {
            Registers = registers.Snapshot();
            Flags = flags.Clone();
            
            Address = instructionPackage.InstructionAddress;
            Instruction = instructionPackage.Instruction;
            Arg1 = instructionPackage.Data.Argument1;
            Arg2 = instructionPackage.Data.Argument2;
            EventName = eventName;

            Disassembly = Instruction.Disassemble(Instruction, Arg1, Arg2);
        }
    }
}

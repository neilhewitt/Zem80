using System.Security.Cryptography;

namespace Zem80.Core.CPU
{
    public class ExecutionState
    {
        public Instruction Instruction { get; }
        public MachineCycle MachineCycle { get; }
        public byte? Arg1 { get; }
        public byte? Arg2 { get; }
        public IRegisters Registers { get; }
        public Flags Flags { get; }

        public ExecutionState(Instruction instruction, byte? arg1, byte? arg2, Flags currentFlags, IRegisters currentRegisters, MachineCycle machineCycle)
        {
            Instruction = instruction;
            Arg1 = arg1;
            Arg2 = arg2;
            Registers = currentRegisters;
            Flags = currentFlags;
            MachineCycle = machineCycle;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace Zem80.Core.CPU
{
    public class InstructionPackage
    {
        public Instruction Instruction { get; private set; }
        public InstructionData Data { get; private set; }
        public ushort InstructionAddress { get; private set; }

        public override string ToString()
        {
            return $"{InstructionAddress.ToString("X4")}: {Instruction.Disassemble(Instruction, Data.Argument1, Data.Argument2)}";
        }

        public InstructionPackage(Instruction instruction, InstructionData data, ushort instructionAddress)
        {
            Instruction = instruction;
            Data = data;
            InstructionAddress = instructionAddress;
        }
    }
}

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

        public (ushort address, string disassembly) Disassemble()
        {
            // pattern = mnemonic, replace single n or o with argument1, replace nn with argument1+argument2 (concat, not comma-separated)
            // using sizeinbytes doesn't work
            // use the mnemonic itself as a template
            string disassembly = Instruction.Mnemonic;
            if (disassembly.Contains("nn"))
            {
                disassembly = disassembly.Replace("nn", "0x" + Data.ArgumentsAsWord.ToString("X4", CultureInfo.InvariantCulture));
            }
            else
            {
                if (disassembly.Contains("n"))
                {
                    disassembly = disassembly.Replace("n", "0x" + Data.Argument1.ToString("X2", CultureInfo.InvariantCulture));
                }
                if (disassembly.Contains("o"))
                {
                    disassembly = disassembly.Replace("o", "0x" + Data.Argument2.ToString("X2", CultureInfo.InvariantCulture));
                }
            }
            
            return (InstructionAddress, disassembly);
        }

        public override string ToString()
        {
            return $"{InstructionAddress.ToString("X4", CultureInfo.InvariantCulture)}: (0x{Instruction.Opcode.ToString("X4", CultureInfo.InvariantCulture)}) {Instruction.Mnemonic} {Data.Argument1.ToString("X2", CultureInfo.InvariantCulture)}, {Data.Argument2.ToString("X2", CultureInfo.InvariantCulture)}";
        }

        public InstructionPackage(Instruction instruction, InstructionData data, ushort instructionAddress)
        {
            Instruction = instruction;
            Data = data;
            InstructionAddress = instructionAddress;
        }
    }
}

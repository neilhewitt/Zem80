using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public abstract class Instruction : IInstruction
    {
        public static IInstruction NOP
        {
            get
            {
                IInstruction nop = new NoArgumentInstruction()
                {
                    Mnemonic = "NOP",
                    MachineCycles = 1,
                    ClockCycles = 1,
                    SizeInBytes = 1
                };
                return nop;
            }
        }

        public byte Opcode { get; private set; }
        public byte Prefix { get; private set; }
        public string OpcodeAsHexString => String.Format("X2", Opcode);
        public string Mnemonic { get; private set; }
        public int SizeInBytes { get; private set; }
        public int MachineCycles { get; private set; }
        public int ClockCycles { get; private set; }
    }
}

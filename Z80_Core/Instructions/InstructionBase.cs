using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public abstract class InstructionBase : IInstruction
    {
        public byte Opcode { get; private set; }
        public byte Prefix { get; private set; }
        public string OpcodeAsHexString => String.Format("X2", Opcode);
        public string Mnemonic { get; private set; }
        public int SizeInBytes { get; private set; }
        public int MachineCycles { get; private set; }
        public int ClockCycles { get; private set; }
    }
}

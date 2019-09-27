using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class NoArgumentInstructionWithOpcodeOffset : InstructionBase, IOpcodeOffset
    {
        public byte Offset { get; private set; }

        public OpcodeOffsetType OffsetType { get; private set; }
    }
}

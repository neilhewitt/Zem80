using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class IndexedInstructionWithOpcodeOffset : IndexedInstruction, IOpcodeOffset
    {
        public byte Offset { get; private set; }
        public OpcodeOffsetType OffsetType { get; private set; }
    }
}

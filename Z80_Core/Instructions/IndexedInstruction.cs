using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class IndexedInstruction : Instruction
    {
        public byte IndexOffset { get; private set; }
    }
}

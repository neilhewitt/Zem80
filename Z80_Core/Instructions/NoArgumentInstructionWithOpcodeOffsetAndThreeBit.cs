using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class NoArgumentInstructionWithOpcodeOffsetAndThreeBit : NoArgumentInstructionWithOpcodeOffset
    {
        public ThreeBit ThreeBit { get; private set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Instructions
{
    public class TwoWordArgumentInstruction : InstructionBase
    {
        public ushort Word1 { get; private set; }
        public ushort Word2 { get; private set; }
    }
}

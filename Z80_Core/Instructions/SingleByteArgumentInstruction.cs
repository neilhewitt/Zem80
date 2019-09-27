using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SingleByteArgumentInstruction : InstructionBase, ISingleByteArgument
    {
        public byte Argument { get; set; }
    }
}

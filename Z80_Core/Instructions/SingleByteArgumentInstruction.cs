using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class SingleByteArgumentInstruction : Instruction, ISingleByteArgument
    {
        public byte Argument { get; set; }
    }
}

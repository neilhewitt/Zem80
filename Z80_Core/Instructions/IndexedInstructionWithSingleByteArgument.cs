using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class IndexedInstructionWithSingleByteArgument : IndexedInstruction, ISingleByteArgument
    {
        public byte Argument { get; private set; }
    }
}

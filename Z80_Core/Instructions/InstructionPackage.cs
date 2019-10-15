using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class InstructionPackage
    {
        public InstructionInfo Info { get; }
        public InstructionData Data { get; }

        public InstructionPackage(InstructionInfo info, InstructionData data)
        {
            Info = info;
            Data = data;
        }
    }
}

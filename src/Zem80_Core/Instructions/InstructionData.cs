using System;
using System.Collections.Generic;
using System.Text;

namespace Zem80.Core.Instructions
{
    public class InstructionData
    {
        public byte Argument1 { get; set; }
        public byte Argument2 { get; set; }
        public ushort ArgumentsAsWord => (ushort)((Argument2 * 256) + Argument1);
    }
}

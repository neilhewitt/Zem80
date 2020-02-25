using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class InstructionData
    {
        public byte Opcode { get; set; }
        public byte? BitIndex { get; set; }
        public RegisterName? Register { get; set; }
        public bool DirectIX { get; set; }
        public bool DirectIY { get; set; }
        public bool IndexIX { get; set; }
        public bool IndexIY { get; set; }
        public byte Argument1 { get; set; }
        public byte Argument2 { get; set; }
        public ushort ArgumentsAsWord => (ushort)((Argument2 * 256) + Argument1);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class InstructionData
    {
        public byte Opcode { get; set; }
        public byte? Displacement { get; set; }
        public uint? BitIndex { get; set; }
        public RegisterIndex? RegisterIndex { get; set; }
        public bool DirectIX { get; set; }
        public bool DirectIY { get; set; }
        public bool IndexIX { get; set; }
        public bool IndexIY { get; set; }
        public byte[] Arguments { get; set; } = new byte[0];
        public ushort ArgumentsAsWord => (ushort)(((Arguments?[1] ?? 0) * 256) + Arguments?[2] ?? 0);
    }
}

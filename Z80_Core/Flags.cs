using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public struct Flags : IFlags
    {
        public bool Carry { get; set; }
        public bool Five { get; set; }
        public bool HalfCarry { get; set; }
        public bool Parity { get; set; }
        public bool Sign { get; set; }
        public bool Subtract { get; set; }
        public bool Three { get; set; }
        public bool Zero { get; set; }
    }
}

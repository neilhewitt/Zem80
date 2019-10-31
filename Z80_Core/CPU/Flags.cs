using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public struct Flags : IFlags
    {
        public static Flags Copy(IFlags input)
        {
            Flags output = new Flags();
            output.Carry = input.Carry;
            output.Five = input.Five;
            output.HalfCarry = input.HalfCarry;
            output.ParityOverflow = input.ParityOverflow;
            output.Sign = input.Sign;
            output.Subtract = input.Subtract;
            output.Three = input.Three;
            output.Zero = input.Zero;
            return output;
        }

        public bool Carry { get; set; }
        public bool Five { get; set; }
        public bool HalfCarry { get; set; }
        public bool ParityOverflow { get; set; }
        public bool Sign { get; set; }
        public bool Subtract { get; set; }
        public bool Three { get; set; }
        public bool Zero { get; set; }
    }
}

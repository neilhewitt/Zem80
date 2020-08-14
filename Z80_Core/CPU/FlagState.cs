using System;

namespace Z80.Core
{
    [Flags]
    public enum FlagState
    {
        None = 0,
        Carry = 1,
        Subtract = 2,
        X = 4,
        ParityOverflow = 8,
        Y = 16,
        HalfCarry = 32,
        Zero = 64,
        Sign = 128
    }
}

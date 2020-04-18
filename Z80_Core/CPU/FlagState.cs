using System;

namespace Z80.Core
{
    [Flags]
    public enum FlagState
    {
        None = 0,
        Carry = 1,
        Subtract = 2,
        Three = 4,
        ParityOverflow = 8,
        Five = 16,
        HalfCarry = 32,
        Zero = 64,
        Sign = 128
    }
}

using System;

namespace Zem80.Core.CPU
{
    [Flags]
    public enum FlagState
    {
        None = 0,
        Carry = 1,
        Subtract = 2,
        ParityOverflow = 4,
        X = 8,
        HalfCarry = 16,
        Y = 32,
        Zero = 64,
        Sign = 128
    }
}

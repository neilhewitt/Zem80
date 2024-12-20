﻿namespace Zem80.Core.CPU
{
    public enum BitwiseOperation
    {
        ShiftLeftSetBit0,
        ShiftLeftResetBit0,
        ShiftRightPreserveBit7,
        ShiftRightResetBit7,
        RotateLeft, 
        RotateRight, 
        RotateLeftThroughCarry, 
        RotateRightThroughCarry, 
        None
    }
}
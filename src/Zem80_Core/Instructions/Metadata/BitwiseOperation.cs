namespace Zem80.Core.CPU
{
    public enum BitwiseOperation
    {
        ShiftLeft, 
        ShiftLeftSetBit0,
        ShiftLeftResetBit0,
        ShiftRight, 
        ShiftRightPreserveBit7,
        ShiftRightResetBit7,
        RotateLeft, 
        RotateRight, 
        RotateLeftThroughCarry, 
        RotateRightThroughCarry, 
        None
    }
}
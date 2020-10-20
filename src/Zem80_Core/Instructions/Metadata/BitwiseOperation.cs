namespace Zem80.Core.Instructions
{
    public enum BitwiseOperation
    {
        ShiftLeft, 
        ShiftLeftSetBit0, 
        ShiftRight, 
        ShiftRightPreserveBit7, 
        RotateLeft, 
        RotateRight, 
        RotateLeftThroughCarry, 
        RotateRightThroughCarry, 
        None
    }
}
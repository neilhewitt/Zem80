using System;

namespace Zem80.Core.CPU
{
    public static class Bitwise
    {
        public static (byte Result, Flags Flags) ShiftLeftSetBit0(this byte value, Flags currentFlags, FlagState? flagsToSet = null)
        {
            return BitwiseFlags(value, BitwiseOperation.ShiftLeftSetBit0, currentFlags, flagsToSet);
        }

        public static (byte Result, Flags Flags) ShiftLeftResetBit0(this byte value, Flags currentFlags, FlagState? flagsToSet = null)
        {
            return BitwiseFlags(value, BitwiseOperation.ShiftLeftResetBit0, currentFlags, flagsToSet);
        }

        public static (byte Result, Flags Flags) ShiftRightPreserveBit7(this byte value, Flags currentFlags, FlagState? flagsToSet = null)
        {
            return BitwiseFlags(value, BitwiseOperation.ShiftRightPreserveBit7, currentFlags, flagsToSet);
        }

        public static (byte Result, Flags Flags) ShiftRightResetBit7(this byte value, Flags currentFlags, FlagState? flagsToSet = null)
        {
            return BitwiseFlags(value, BitwiseOperation.ShiftRightResetBit7, currentFlags, flagsToSet);
        }

        public static (byte Result, Flags Flags) RotateLeft(this byte value, Flags currentFlags, FlagState? flagsToSet = null)
        {
            return BitwiseFlags(value, BitwiseOperation.RotateLeft, currentFlags, flagsToSet);
        }

        public static (byte Result, Flags Flags) RotateRight(this byte value, Flags currentFlags, FlagState? flagsToSet = null)
        {
            return BitwiseFlags(value, BitwiseOperation.RotateRight, currentFlags, flagsToSet);
        }

        public static (byte Result, Flags Flags) RotateLeftThroughCarry(this byte value, Flags currentFlags, FlagState? flagsToSet = null)
        {
            return BitwiseFlags(value, BitwiseOperation.RotateLeftThroughCarry, currentFlags, flagsToSet);
        }

        public static (byte Result, Flags Flags) RotateRightThroughCarry(this byte value, Flags currentFlags, FlagState? flagsToSet = null)
        {
            return BitwiseFlags(value, BitwiseOperation.RotateRightThroughCarry, currentFlags, flagsToSet);
        }

        private static (byte Result, Flags Flags) BitwiseFlags(byte value, BitwiseOperation operation, Flags flags, FlagState? flagsToSet)
        {
            // some bitwise instructions don't set all flags; the FlagState parameter allows the caller to specify which flags to set
            // we turn this into a Flags object here so we can easily check for the presence of flags
            Flags set = new Flags(flagsToSet ?? Flags.All); // if no FlagState is provided, set all flags

            byte leftCarry = (byte)(flags.Carry ? 0x01 : 0);
            byte rightCarry = (byte)(flags.Carry ? 0x80 : 0);

            int result = operation switch
            {
                BitwiseOperation.ShiftLeftSetBit0 => (value << 1) + 1,
                BitwiseOperation.ShiftLeftResetBit0 => ((value << 1) & ~0x01),
                BitwiseOperation.ShiftRightPreserveBit7 => ((byte)(value >> 1 | (value & 0x80))),
                BitwiseOperation.ShiftRightResetBit7 => ((byte)((value >> 1) & ~0x80)),
                BitwiseOperation.RotateLeft => ((byte)(value << 1 | value >> 7)),
                BitwiseOperation.RotateRight => ((byte)(value >> 1 | value << 7)),
                BitwiseOperation.RotateLeftThroughCarry => ((byte)(value << 1 | leftCarry)),
                BitwiseOperation.RotateRightThroughCarry => ((byte)(value >> 1 | rightCarry)),
                _ => value
            };

            if (set.Carry)
            {
                flags.Carry = operation switch
                {
                    BitwiseOperation.ShiftLeftSetBit0 => (byte)(value & 0x80) > 0,
                    BitwiseOperation.ShiftLeftResetBit0 => (byte)(value & 0x80) > 0,
                    BitwiseOperation.ShiftRightPreserveBit7 => (byte)(value & 0x01) > 0,
                    BitwiseOperation.ShiftRightResetBit7 => (byte)(value & 0x01) > 0,
                    BitwiseOperation.RotateLeft => (byte)(value & 0x80) > 0,
                    BitwiseOperation.RotateRight => (byte)(value & 0x01) > 0,
                    BitwiseOperation.RotateLeftThroughCarry => (byte)(value & 0x80) > 0,
                    BitwiseOperation.RotateRightThroughCarry => (byte)(value & 0x01) > 0,
                    _ => flags.Carry
                };
            }

            byte output = (byte)result;

            if (set.Zero) flags.Zero = ((byte)result == 0x00);
            if (set.Sign) flags.Sign = (((sbyte)result) < 0);
            if (set.ParityOverflow) flags.ParityOverflow = ((byte)result).EvenParity();
            if (set.HalfCarry) flags.HalfCarry = false;
            if (set.Subtract) flags.Subtract = false;
            if (set.X) flags.X = (result & 0x08) > 0; // copy bit 3
            if (set.Y) flags.Y = (result & 0x20) > 0; // copy bit 5
            
            return (output, flags);
        }
    }
}

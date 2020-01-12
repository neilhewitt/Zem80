using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Z80.Core
{
    public static class Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHexString(this byte input)
        {
            return input.ToString("X2");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte LowByte(this ushort input)
        {
            return (byte)(input % 256);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte HighByte(this ushort input)
        {
            return (byte)(input / 256);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HalfCarryWhenAdding(this byte first, byte second)
        {
            return ((first & 0x0F) + (second & 0x0F) & 0x10) == 0x10;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HalfCarryWhenSubtracting(this byte first, byte second)
        {
            return ((first & 0x0F) - (second & 0x0F) < 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HalfCarry(this sbyte value)
        {
            return (((value & 0x0F) & 0x10) != 0x10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OverflowsWhenAdding(this byte first, byte second)
        {
            short result = (short)(first + second);
            return ((first > 0x80 && second > 0x80 && result > 0) 
                || (first < 0x80 && second < 0x80 && result < 0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OverflowsWhenSubtracting(this byte first, byte second)
        {
            return (short)(first - second) < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte RemoveBits(this byte input, int startIndex, int numberOfBits)
        {
            ByteBits bits = new ByteBits(input);
            bits.SetBits(startIndex, numberOfBits, false);
            return bits.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool[] GetBits(this byte input, int startIndex, int numberOfBits)
        {
            ByteBits bits = new ByteBits(input);
            bool[] output = new bool[numberOfBits];
            for (int i = startIndex; i < startIndex + numberOfBits; i++)
            {
                output[i - startIndex] = bits[i];
            }
            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetByteFromBits(this byte input, int startIndex, int numberOfBits)
        {
            ByteBits bits = new ByteBits(input);
            return ((byte)0x00).SetBits(startIndex, bits.GetBits(startIndex, numberOfBits));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte SetBits(this byte input, int startIndex, params bool[] bitsToSet)
        {
            ByteBits bits = new ByteBits(input);
            for(int i = startIndex; i < startIndex + bitsToSet.Length; i++)
            {
                bits[i] = bitsToSet[i - startIndex];
            }
            return bits.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte SetBit(this byte input, int bitIndex, bool state)
        {
            ByteBits bits = new ByteBits(input);
            bits[bitIndex] = state;
            return bits.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBit(this byte input, int bitIndex)
        {
            ByteBits bits = new ByteBits(input);
            return bits[bitIndex];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte CountBits(this byte input, bool state)
        {
            if (!state) input = (byte)~input;
            byte bits = 0;
            while (input > 0)
            {
                bits += (byte)(input & 1);
                input >>= 1;
            }

            return bits;
        }
    }
}

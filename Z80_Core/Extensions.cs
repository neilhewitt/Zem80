using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public static string ToBinaryString(this byte input)
        {
            return new ByteBits(input).ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte LowByte(this ushort input)
        {
            return (byte)(input % 256); 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte HighByte(this ushort input)
        {
            return (byte)(input / 256); // note this will always be valid, even on big-endian architectures
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HalfCarryWhenAdding(this byte first, byte second)
        {
            return (((first & 0x0F) + (second & 0x0F)) & 0x10) == 0x10;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HalfCarryWhenAdding(this ushort first, ushort second)
        {
            return (((first & 0x0FFF) + (second & 0x0FFF)) & 0x1000) == 0x1000;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HalfCarryWhenSubtracting(this byte first, byte second)
        {
            return ((first & 0x0F) - (second & 0x0F) < 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HalfCarryWhenConvertingToByte(this sbyte value)
        {
            return (((value & 0x0F) & 0x10) != 0x10);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OverflowsWhenAdding(this byte first, byte second)
        {
            int result = (byte)((sbyte)first + (sbyte)second);
            return (result >= 0x80 || result <= -0x80);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OverflowsWhenAdding(this ushort first, ushort second)
        {
            int result = (byte)((sbyte)first + (sbyte)second);
            return (result >= 0x8000 || result <= -0x8000);
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
        public static bool[] GetHighNybble(this byte input)
        {
            return input.GetBits(4, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool[] GetLowNybble(this byte input)
        {
            return input.GetBits(0, 4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte SetHighNybble(this byte input, bool[] bits)
        {
            if (bits.Length != 4) throw new IndexOutOfRangeException();
            return input.SetBits(4, bits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte SetLowNybble(this byte input, bool[] bits)
        {
            if (bits.Length != 4) throw new IndexOutOfRangeException();
            return input.SetBits(0, bits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetByteFromBits(this byte input, int startIndex, int numberOfBits)
        {
            ByteBits bits = new ByteBits(input);
            return ((byte)0x00).SetBits(0, bits.GetBits(startIndex, numberOfBits));
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
            return state switch
            {
                true => (byte)(input | (1 << bitIndex)),
                false => (byte)(input | (0 << bitIndex))
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBit(this byte input, int bitIndex)
        {
            return (input & (1 << bitIndex)) != 0;
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

        public static string ToBinaryString(this bool[] input)
        {
            return String.Join("", input.Reverse().Select(x => x ? "1" : "0"));
        }
    }
}

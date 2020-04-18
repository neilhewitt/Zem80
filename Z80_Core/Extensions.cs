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
        public static bool[] GetBits(this byte input, int startIndex, int numberOfBits)
        {
            bool[] output = new bool[numberOfBits];
            for (int i = startIndex; i < startIndex + numberOfBits; i++)
            {
                output[i - startIndex] = input.GetBit(i);
            }
            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte SetBits(this byte input, int startIndex, params bool[] bitsToSet)
        {
            byte output = input;
            for (int i = startIndex; i < startIndex + bitsToSet.Length; i++)
            {
                output.SetBit(i, bitsToSet[i - startIndex]);
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
            return ((byte)0x00).SetBits(0, input.GetBits(startIndex, numberOfBits));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte SetBit(this byte input, int bitIndex, bool state)
        {
            return state switch
            {
                true => (byte)(input | (1 << bitIndex)),
                false => (byte)(input & ~(1 << bitIndex))
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort SetBit(this ushort input, int bitIndex, bool state)
        {
            return state switch
            {
                true => (ushort)(input | (1 << bitIndex)),
                false => (ushort)(input & ~(1 << bitIndex))
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBit(this byte input, int bitIndex)
        {
            return (input & (1 << bitIndex)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetBit(this ushort input, int bitIndex)
        {
            return (input & (1 << bitIndex)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CountBits(this byte input, bool state)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string SetChar(this string input, int index, char replace)
        {
            if (input.Length <= index) throw new ArgumentOutOfRangeException();
            if (index == 0) return replace + input.Substring(1); // start
            if (index == input.Length - 1) return input.Substring(0, index) + replace; // end
            else return input.Substring(0, index) + replace + input.Substring(index + 1);
        }
    }
}

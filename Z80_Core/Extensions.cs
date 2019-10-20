using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public static class Extensions
    {
        public static string ToHexString(this byte input)
        {
            return input.ToString("X2");
        }

        public static byte RemoveBits(this byte input, int startIndex, int numberOfBits)
        {
            ByteBits bits = new ByteBits(input);
            bits.SetBits(startIndex, numberOfBits, false);
            return bits.Value;
        }

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

        public static byte SetBits(this byte input, int startIndex, params bool[] bitsToSet)
        {
            ByteBits bits = new ByteBits(input);
            for(int i = startIndex; i < startIndex + bitsToSet.Length; i++)
            {
                bits[i] = bitsToSet[i - startIndex];
            }
            return bits.Value;
        }

        public static byte SetBit(this byte input, int bitIndex, bool state)
        {
            ByteBits bits = new ByteBits(input);
            bits[bitIndex] = state;
            return bits.Value;
        }

        public static bool GetBit(this byte input, int bitIndex)
        {
            ByteBits bits = new ByteBits(input);
            return bits[bitIndex];
        } 
        
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Zem80.Core
{
    public static class ArithmeticExtensions
    {

        public static byte LowByte(this ushort input)
        {
            return (byte)(input % 256); 
        }


        public static byte HighByte(this ushort input)
        {
            return (byte)(input / 256); // note this will always be valid, even on big-endian architectures
        }


        public static bool[] GetBits(this byte input, int startIndex, int numberOfBits)
        {
            bool[] output = new bool[numberOfBits];
            for (int i = startIndex; i < startIndex + numberOfBits; i++)
            {
                output[i - startIndex] = input.GetBit(i);
            }
            return output;
        }


        public static byte SetBits(this byte input, int startIndex, params bool[] bitsToSet)
        {
            byte output = input;
            for (int i = startIndex; i < startIndex + bitsToSet.Length; i++)
            {
                output = output.SetBit(i, bitsToSet[i - startIndex]);
            }
            return output;        
        }


        public static bool[] GetHighNybble(this byte input)
        {
            return input.GetBits(4, 4);
        }


        public static bool[] GetLowNybble(this byte input)
        {
            return input.GetBits(0, 4);
        }


        public static byte SetHighNybble(this byte input, bool[] bits)
        {
            if (bits.Length != 4) throw new IndexOutOfRangeException();
            return input.SetBits(4, bits);
        }


        public static byte SetLowNybble(this byte input, bool[] bits)
        {
            if (bits.Length != 4) throw new IndexOutOfRangeException();
            return input.SetBits(0, bits);
        }


        public static byte GetByteFromBits(this byte input, int startIndex, int numberOfBits)
        {
            return ((byte)0x00).SetBits(0, input.GetBits(startIndex, numberOfBits));
        }


        public static byte SetBit(this byte input, int bitIndex, bool state)
        {
            return state switch
            {
                true => (byte)(input | (1 << bitIndex)),
                false => (byte)(input & ~(1 << bitIndex))
            };
        }


        public static ushort SetBit(this ushort input, int bitIndex, bool state)
        {
            return state switch
            {
                true => (ushort)(input | (1 << bitIndex)),
                false => (ushort)(input & ~(1 << bitIndex))
            };
        }


        public static bool GetBit(this byte input, int bitIndex)
        {
            return (input & (1 << bitIndex)) != 0;
        }


        public static bool GetBit(this ushort input, int bitIndex)
        {
            return (input & (1 << bitIndex)) != 0;
        }


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

        public static bool EvenParity(this byte input)
        {
            return input.CountBits(true) % 2 == 0;
        }

        public static ushort ToWord(this (byte low, byte high) bytePair)
        {
            return (ushort)((bytePair.high * 256) + bytePair.low);
        }
    }
}

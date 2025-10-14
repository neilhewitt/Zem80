using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core
{
    public static class ByteAndWordExtensions
    {
        public static byte LowByte(this ushort input)
        {
            return (byte)(input % 256); 
        }

        public static byte HighByte(this ushort input)
        {
            return (byte)(input / 256); // note this will always be valid, even on big-endian architectures
        }

        public static byte GetHighNybble(this byte input)
        {
            return (byte)((input & 0xF0) >> 4); // returns high nybble of input as low nybble of a new byte
        }

        public static byte GetLowNybble(this byte input)
        {
            return (byte)(input & 0x0F); // returns lowest 4 bits of input as a new byte
        }

        public static byte SetHighNybble(this byte input, byte from)
        {
            // NOTE: assumes the required 4 bits are the lowest 4 bits of from; upper 4 bits are discarded
            from = (byte)(from << 4); // shift low to high
            input = (byte)(input & 0x0F); // clear high
            input = (byte)(from | input);
            return input;
        }

        public static byte SetLowNybble(this byte input, byte from)
        {
            // NOTE: overwrites the lowest 4 bits of input with the lowest 4 bits of from
            return (byte)(((byte)input & 0xF0) | ((byte)from & 0x0F));
        }

        public static byte GetByteFromBits(this byte input, int startIndex, int numberOfBits)
        {
            return (byte)((input >> startIndex) & (byte)((1 << numberOfBits) - 1));
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

        public static byte Invert(this byte input)
        {
            return (byte)(input ^ 0xFF);
        }

        public static bool EvenParity(this byte input)
        {
            // NOTE 10/11/24:
            // OK, I'm going to admit that I took this code from GPT.
            // I only sort-of understand how it works - but I *have* verified that it does.
            // I wanted a way to count bits without using any loops... and *tada*!
            // This is more performant since EvenParity is called by several instructions
            // and I had identified this method as a bottleneck

            int setBits = input - ((input >> 1) & 0x55); // pairwise count
            setBits = (setBits & 0x33) + ((setBits >> 2) & 0x33); // nybblewise count
            setBits = (setBits + (setBits >> 4)) & 0x0F;  // bytewise count
            setBits = setBits & 0x0F;  // only the lower 4 bits are retained (since max count is 8 = 1000 in binary)

            return setBits % 2 == 0;
        }

        public static ushort ToWord(this (byte low, byte high) bytePair)
        {
            return (ushort)((bytePair.high * 256) + bytePair.low);
        }
    }
}

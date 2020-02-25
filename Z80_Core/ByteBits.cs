using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ByteBits
    {
        private BitArray _bits;

        public bool this[int bitIndex]
        {
            get
            {
                return _bits[bitIndex];
            }
            set
            {
                _bits[bitIndex] = value;
            }
        }

        public byte Value
        {
            get
            {
                byte[] output = new byte[1];
                _bits.CopyTo(output, 0);
                return output[0];
            }
        }

        public void SetBits(int startIndex, int length, bool value)
        {
            for (int i = startIndex; i < startIndex + length; i++)
            {
                _bits[i] = value;
            }
        }

        public bool[] GetBits(int startIndex, int length)
        {
            bool[] bits = new bool[length];
            for (int i = startIndex; i < startIndex + length; i++)
            {
                bits[i - startIndex] = _bits[i];
            }
            return bits;
        }

        public override string ToString()
        {
            string bits = "";
            for(int i = 7; i >= 0; i--)
            {
                bits += _bits[i] ? "1" : "0";
            }
            return bits;
        }

        public ByteBits(byte value)
        {
            _bits = new BitArray(new byte[1] { value });
        }
    }
}

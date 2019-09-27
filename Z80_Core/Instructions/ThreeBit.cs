using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class ThreeBit
    {
        public bool Bit1 { get; private set; }
        public bool Bit2 { get; private set; }
        public bool Bit3 { get; private set; }

        public byte ToByte()
        {
            return Convert.ToByte("00000" + Bit(Bit1) + Bit(Bit2) + Bit(Bit3));

            string Bit(bool bit)
            {
                return bit ? "1" : "0";
            }
        }

        public ThreeBit(byte input)
        {
            BitArray bits = new BitArray(new byte[] { input });
            Bit1 = bits[5];
            Bit2 = bits[6];
            Bit3 = bits[7];
        }

        public ThreeBit()
        {
        }
    }
}

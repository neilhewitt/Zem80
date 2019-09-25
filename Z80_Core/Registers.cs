using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class Registers
    {
        private byte[] _registerStorage = new byte[27];

        // 8-bit registers
        public byte A { get { return _registerStorage[0]; } set { _registerStorage[0] = value; } }
        public byte F { get { return _registerStorage[1]; } private set { _registerStorage[1] = value; } } // flags register: cannot be directly set
        public byte B { get { return _registerStorage[2]; } set { _registerStorage[2] = value; } }
        public byte C { get { return _registerStorage[3]; } set { _registerStorage[3] = value; } }
        public byte D { get { return _registerStorage[4]; } set { _registerStorage[4] = value; } }
        public byte E { get { return _registerStorage[5]; } set { _registerStorage[5] = value; } }
        public byte H { get { return _registerStorage[6]; } set { _registerStorage[6] = value; } }
        public byte L { get { return _registerStorage[7]; } set { _registerStorage[7] = value; } }

        // 16-bit pairs
        public ushort AF { get { return Get16BitValue(0); } } // cannot set value as 16-bit register due to flags, use LD A, x instead
        public ushort BC { get { return Get16BitValue(2); } set { Set16BitValue(2, value); } }
        public ushort DE { get { return Get16BitValue(4); } set { Set16BitValue(4, value); } }
        public ushort HL { get { return Get16BitValue(6); } set { Set16BitValue(6, value); } }

        // 16-bit registers
        public ushort IX { get { return Get16BitValue(16); } set { Set16BitValue(16, value); } }
        public ushort IY { get { return Get16BitValue(18); } set { Set16BitValue(18, value); } }
        public ushort SP { get { return Get16BitValue(20); } set { Set16BitValue(20, value); } }

        // 8-bit 'other' registers
        public byte I { get { return _registerStorage[22]; } set { _registerStorage[22] = value; } }
        public byte R { get { return _registerStorage[23]; } set { _registerStorage[23] = value; } }

        // program counter
        public ushort PC { get { return Get16BitValue(24); } private set { Set16BitValue(value, 24); } } // cannot set value directly

        public void ExchangeAF()
        {
            ExchangePair(0);
        }

        public void ExchangeAll()
        {
            for(int i = 0; i < 8; i+=2)
            {
                ExchangePair(i);
            }
        }

        private ushort Get16BitValue(int registerIndex)
        {
            return BitConverter.ToUInt16(_registerStorage, registerIndex);
        }

        private void Set16BitValue(int registerIndex, ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            _registerStorage[registerIndex] = bytes[0]; _registerStorage[registerIndex+1] = bytes[1];
        }

        private void ExchangePair(int registerIndex)
        {
            int altRegisterIndex = registerIndex + 8;

            ushort alternateValue = Get16BitValue(altRegisterIndex);
            Set16BitValue(altRegisterIndex, Get16BitValue(registerIndex));
            Set16BitValue(registerIndex, alternateValue);
        }
    }
}

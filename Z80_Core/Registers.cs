using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class Registers : IRegisters
    {
        private byte[] _registers = new byte[26];
        private byte _AFOffset = 0;
        private byte _BCDEHLOffset = 0;

        // 8-bit registers
        public byte A { get { return _registers[_AFOffset]; } set { _registers[_AFOffset] = value; } }
        public byte F { get { return _registers[_AFOffset + 1]; } private set { _registers[_AFOffset + 1] = value; } } // flags register: cannot be directly set
        public byte B { get { return _registers[_BCDEHLOffset + 2]; } set { _registers[_BCDEHLOffset + 2] = value; } }
        public byte C { get { return _registers[_BCDEHLOffset + 3]; } set { _registers[_BCDEHLOffset + 3] = value; } }
        public byte D { get { return _registers[_BCDEHLOffset + 4]; } set { _registers[_BCDEHLOffset + 4] = value; } }
        public byte E { get { return _registers[_BCDEHLOffset + 5]; } set { _registers[_BCDEHLOffset + 5] = value; } }
        public byte H { get { return _registers[_BCDEHLOffset + 6]; } set { _registers[_BCDEHLOffset + 6] = value; } }
        public byte L { get { return _registers[_BCDEHLOffset + 7]; } set { _registers[_BCDEHLOffset + 7] = value; } }

        // Registers as 16-bit pairs
        public ushort AF { get { return Get16BitValue(_AFOffset); } } // cannot set value as 16-bit register due to flags, use LD A, x instead
        public ushort BC { get { return Get16BitValue(_BCDEHLOffset + 2); } set { Set16BitValue(_BCDEHLOffset + 2, value); } }
        public ushort DE { get { return Get16BitValue(_BCDEHLOffset + 4); } set { Set16BitValue(_BCDEHLOffset + 4, value); } }
        public ushort HL { get { return Get16BitValue(_BCDEHLOffset + 6); } set { Set16BitValue(_BCDEHLOffset + 6, value); } }

        // There is a second 'shadow' bank of register values (AF', BC', DE', HL'). These are stored in _registers[8..15].
        // To access these you call ExchangeAF (to get access to values in AF') or ExchangeBCDEHL (to get access to values in BC', DE' and HL').
        // For speed, these operations just set an offset value so that a read instruction on A, say, accesses _registers[8] instead of _registers[0].

        // 16-bit special registers (index, special, IR, program counter)
        public ushort IX { get { return Get16BitValue(16); } set { Set16BitValue(16, value); } }
        public ushort IY { get { return Get16BitValue(18); } set { Set16BitValue(18, value); } }
        public ushort SP { get { return Get16BitValue(20); } set { Set16BitValue(20, value); } }

        // 8-bit 'other' registers
        public byte I { get { return _registers[22]; } set { _registers[22] = value; } }
        public byte R { get { return _registers[23]; } set { _registers[23] = value; } }

        // program counter
        public ushort PC { get { return Get16BitValue(24); } private set { Set16BitValue(value, 24); } } // program counter - cannot set value directly

        public void ExchangeAF()
        {
            _AFOffset = (byte)((_AFOffset == 0) ? 8 : 0);
        }

        public void ExchangeBCDEHL()
        {
            _BCDEHLOffset = (byte)((_BCDEHLOffset == 0) ? 8 : 0);
        }

        public Registers Snapshot()
        {
            return new Registers(_registers);
        }

        private ushort Get16BitValue(int registerIndex)
        {
            return BitConverter.ToUInt16(_registers, registerIndex);
        }

        private void Set16BitValue(int registerIndex, ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            _registers[registerIndex] = bytes[0]; _registers[registerIndex + 1] = bytes[1];
        }

        private void ExchangePair(int registerIndex)
        {
            int altRegisterIndex = registerIndex + 8;

            ushort alternateValue = Get16BitValue(altRegisterIndex);
            Set16BitValue(altRegisterIndex, Get16BitValue(registerIndex));
            Set16BitValue(registerIndex, alternateValue);
        }

        public Registers()
        {
        }

        private Registers(byte[] registerValues)
        {
            _registers = registerValues;
        }
    }
}

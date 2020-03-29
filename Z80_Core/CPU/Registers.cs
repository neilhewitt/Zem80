using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class Registers : IRegisters, IDebugRegisters
    {
        private byte _accumulator;
        private byte _altAccumulator;
        private byte[] _registers;
        private byte _BCDEHLOffset = 0;
        private Flags _flags;
        private Flags _altFlags;

        public byte this[ByteRegister register] { get { return GetRegister(register); } set { SetRegister(register, value); } }
        public ushort this[WordRegister registerPair] { get { return GetRegisterPair(registerPair); } set { SetRegisterPair(registerPair, value); } }


        // 8-bit registers
        public byte A { get { return _accumulator; } set { _accumulator = value; } }
        public byte F { get { return _flags.Value; } } // flags register - shouldn't set F or AF directly, use Flags property instead
        public byte B { get { return _registers[_BCDEHLOffset + 2]; } set { _registers[_BCDEHLOffset + 2] = value; } }
        public byte C { get { return _registers[_BCDEHLOffset + 3]; } set { _registers[_BCDEHLOffset + 3] = value; } }
        public byte D { get { return _registers[_BCDEHLOffset + 4]; } set { _registers[_BCDEHLOffset + 4] = value; } }
        public byte E { get { return _registers[_BCDEHLOffset + 5]; } set { _registers[_BCDEHLOffset + 5] = value; } }
        public byte H { get { return _registers[_BCDEHLOffset + 6]; } set { _registers[_BCDEHLOffset + 6] = value; } }
        public byte L { get { return _registers[_BCDEHLOffset + 7]; } set { _registers[_BCDEHLOffset + 7] = value; } }

        // Registers as 16-bit pairs
        public ushort AF { get { return GetWord(_accumulator, _flags.Value); } }

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

        // high/low bytes of IX/IY
        public byte IXh { get { return _registers[16]; } set { _registers[16] = value; } }
        public byte IXl { get { return _registers[17]; } set { _registers[17] = value; } }
        public byte IYh { get { return _registers[18]; } set { _registers[18] = value; } }
        public byte IYl { get { return _registers[19]; } set { _registers[19] = value; } }

        // 8-bit 'other' registers
        public byte I { get { return _registers[22]; } set { _registers[22] = value; } }
        public byte R { get { return _registers[23]; } set { _registers[23] = value; } }

        // program counter
        public ushort PC { get { return Get16BitValue(24); } set { Set16BitValue(24, value); } }

        public Flags Flags => _flags;

        // debug property where AF can be set directly (needed for tests)
        ushort IDebugRegisters.AF { get { return GetWord(_accumulator, _flags.Value); } 
                                    set { _accumulator = value.HighByte(); _flags.Value = value.LowByte(); } }

        public void ExchangeAF()
        {
            lock(this)
            {
                byte accumulator = _accumulator;
                _accumulator = _altAccumulator;
                _altAccumulator = accumulator;

                Flags flags = _flags;
                _flags = _altFlags;
                _altFlags = flags;
            }
        }

        public void ExchangeBCDEHL()
        {
            _BCDEHLOffset = (byte)((_BCDEHLOffset == 0) ? 8 : 0);
        }

        public Registers Snapshot()
        {
            return new Registers(_registers);
        }

        public void Clear()
        {
            _registers = new byte[26];
            _flags = new Flags();
            _altFlags = new Flags();
            _accumulator = 0x00;
            _altAccumulator = 0x00;
        }

        private byte GetRegister(ByteRegister index)
        {
            if (index == ByteRegister.None) return 0xFF;

            if (index == ByteRegister.A)
            {
                return _accumulator;
            }
            else
            {
                return _registers[_BCDEHLOffset + (int)index + 2];
            }
        }

        private ushort GetRegisterPair(WordRegister index)
        {
            if (index == WordRegister.None) return 0xFF;

            if (index == WordRegister.AF)
            {
                return GetWord(_accumulator, _flags.Value);
            }
            else
            {
                return Get16BitValue(_BCDEHLOffset + (int)index + 2);
            }
        }

        private void SetRegister(ByteRegister register, byte value)
        {
            if (register != ByteRegister.None)
            {
                if (register == ByteRegister.A)
                {
                    _accumulator = value;
                }
                else
                {
                    _registers[_BCDEHLOffset + (int)register + 2] = value;
                }
            }
        }

        private void SetRegisterPair(WordRegister registerPair, ushort value)
        {
            if (registerPair != WordRegister.None)
            {
                if (registerPair == WordRegister.AF)
                {
                    _accumulator = value.HighByte();
                    _flags.Value = value.LowByte();
                }
                else
                {
                    Set16BitValue(_BCDEHLOffset + (int)registerPair + 2, value);
                }
            }
        }

        private ushort Get16BitValue(int offset)
        {
            return GetWord(_registers[offset], _registers[offset + 1]);
        }

        private ushort GetWord(byte high, byte low)
        {
            return (ushort)((high * 256) + low); 
        }

        private void Set16BitValue(int offset, ushort value)
        {
            _registers[offset] = value.HighByte();
            _registers[offset + 1] = value.LowByte();
        }

        public Registers()
        {
            _registers = new byte[26];
            _flags = new Flags();
            _altFlags = new Flags();
        }

        private Registers(byte[] registerValues)
        {
            if (registerValues.Length != 26)
            {
                throw new ArgumentOutOfRangeException("Invalid format. Size of array registerValues must be 26 bytes.");

            }
            
            _registers = registerValues;
            _flags = new Flags();
            _altFlags = new Flags();
        }
    }
}

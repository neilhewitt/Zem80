using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Z80.Core
{
    public class Registers : IRegisters
    {
        private byte[] _registers;
        private byte _AFOffset = 0;
        private byte _BCDEHLOffset = 0;

        public byte this[Register register] { get { return GetRegister(register); } set { SetRegister(register, value); } }
        public ushort this[RegisterPair registerPair] { get { return GetRegisterPair(registerPair); } set { SetRegisterPair(registerPair, value); } }


        // 8-bit registers
        public byte A { get { return _registers[_AFOffset]; } set { _registers[_AFOffset] = value; } }
        public byte F { get { return _registers[_AFOffset + 1]; } set { _registers[_AFOffset + 1] = value; } } // flags register - shouldn't set F or AF directly, use Flags property instead
        public byte B { get { return _registers[_BCDEHLOffset + 2]; } set { _registers[_BCDEHLOffset + 2] = value; } }
        public byte C { get { return _registers[_BCDEHLOffset + 3]; } set { _registers[_BCDEHLOffset + 3] = value; } }
        public byte D { get { return _registers[_BCDEHLOffset + 4]; } set { _registers[_BCDEHLOffset + 4] = value; } }
        public byte E { get { return _registers[_BCDEHLOffset + 5]; } set { _registers[_BCDEHLOffset + 5] = value; } }
        public byte H { get { return _registers[_BCDEHLOffset + 6]; } set { _registers[_BCDEHLOffset + 6] = value; } }
        public byte L { get { return _registers[_BCDEHLOffset + 7]; } set { _registers[_BCDEHLOffset + 7] = value; } }

        // Registers as 16-bit pairs
        public ushort AF { get { return Get16BitValue(_AFOffset); } set { Set16BitValue(_AFOffset, value); } }
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

        public IFlags Flags { get; private set; }

        public void SetFlags(IFlags flags)
        {
            ((RegisterFlags)Flags).SetFrom(flags);
        }
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

        public void Clear()
        {
            _registers = new byte[26];
        }

        private byte GetRegister(Register index)
        {
            if (index == Register.None) return 0xFF;

            if (index == Register.A)
            {
                return _registers[_AFOffset];
            }
            else
            {
                return _registers[_BCDEHLOffset + (int)index + 2];
            }
        }

        private ushort GetRegisterPair(RegisterPair index)
        {
            if (index == RegisterPair.None) return 0xFF;

            if (index == RegisterPair.AF)
            {
                return Get16BitValue(_AFOffset);
            }
            else
            {
                return Get16BitValue(_BCDEHLOffset + (int)index + 2);
            }
        }

        private void SetRegister(Register register, byte value)
        {
            if (register != Register.None)
            {
                if (register == Register.A)
                {
                    _registers[_AFOffset] = value;
                }
                else
                {
                    _registers[_BCDEHLOffset + (int)register + 2] = value;
                }
            }
        }

        private void SetRegisterPair(RegisterPair registerPair, ushort value)
        {
            if (registerPair != RegisterPair.None)
            {
                if (registerPair == RegisterPair.AF)
                {
                    Set16BitValue(_AFOffset, value);
                }
                else
                {
                    Set16BitValue(_BCDEHLOffset + (int)registerPair + 2, value);
                }
            }
        }

        private ushort Get16BitValue(int offset)
        {
            return (ushort)((_registers[offset] * 256) + _registers[offset + 1]);
        }

        private void Set16BitValue(int offset, ushort value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            bool isLittleEndian = BitConverter.IsLittleEndian;
            // storage of register data is always in little-endian format (as both the Z80 and x86 are little-endian)
            // but .NET is portable so this code *could* be running on a big-endian architecture and the ushort value will come out
            // in reverse order... so set the bytes directly (this is why we're using BitConverter here, because it knows the endian-ness)

            _registers[offset] = bytes[isLittleEndian ? 1 : 0]; 
            _registers[offset + 1] = bytes[isLittleEndian ? 0 : 1];
        }

        public Registers()
        {
            _registers = new byte[26];
            Flags = new RegisterFlags(this);
        }

        private Registers(byte[] registerValues)
        {
            if (registerValues.Length != 26)
            {
                throw new ArgumentOutOfRangeException("Invalid format. Size of array registerValues must be 26 bytes.");

            }
            
            _registers = registerValues;
            Flags = new RegisterFlags(this);
        }
    }
}

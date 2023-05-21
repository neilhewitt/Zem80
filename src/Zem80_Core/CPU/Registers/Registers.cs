using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Zem80.Core.CPU
{
    public class Registers : IShadowRegisters
    {
        private byte[] _registers;

        public byte this[ByteRegister register] { get { return GetRegister(register); } set { SetRegister(register, value); } }
        public ushort this[WordRegister registerPair] { get { return GetRegisterPair(registerPair); } set { SetRegisterPair(registerPair, value); } }

        public IShadowRegisters Shadow => this;

        // 8-bit registers
        public byte A { get { return _registers[0]; } set { _registers[0] = value; } }
        public byte F { get { return _registers[1]; } set { _registers[1] = value; } }
        public byte B { get { return _registers[2]; } set { _registers[2] = value; } }
        public byte C { get { return _registers[3]; } set { _registers[3] = value; } }
        public byte D { get { return _registers[4]; } set { _registers[4] = value; } }
        public byte E { get { return _registers[5]; } set { _registers[5] = value; } }
        public byte H { get { return _registers[6]; } set { _registers[6] = value; } }
        public byte L { get { return _registers[7]; } set { _registers[7] = value; } }

        // Registers as 16-bit pairs
        public ushort AF { get { return Get16BitValue(0); } set { Set16BitValue(0, value); } }
        public ushort BC { get { return Get16BitValue(2); } set { Set16BitValue(2, value); } }
        public ushort DE { get { return Get16BitValue(4); } set { Set16BitValue(4, value); } }
        public ushort HL { get { return Get16BitValue(6); } set { Set16BitValue(6, value); } }

        // There is a second 'shadow' bank of register values (AF', BC', DE', HL'). These are stored in _registers[8..15].
        // To access these you call ExchangeAF (to get access to values in AF') or ExchangeBCDEHL (to get access to values in BC', DE' and HL'). But for debug purposes we can
        // access them directly by casting to the IShadowRegisters interface (or by using the Shadow property, preferably)
        ushort IShadowRegisters.AF { get { return Get16BitValue(8); } set { Set16BitValue(8, value); } }
        ushort IShadowRegisters.BC { get { return Get16BitValue(10); } set { Set16BitValue(10, value); } }
        ushort IShadowRegisters.DE { get { return Get16BitValue(12); } set { Set16BitValue(12, value); } }
        ushort IShadowRegisters.HL { get { return Get16BitValue(14); } set { Set16BitValue(14, value); } }

        // 16-bit special registers (index, stack pointer)
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

        // I+R as a 16-bit pair
        public ushort IR { get { return Get16BitValue(22); } }

        // program counter
        public ushort PC { get { return Get16BitValue(24); } set { Set16BitValue(24, value); } }

        internal ushort WZ { get; set; } // internal register, never exposed to the outside world and not saved / restored with snapshot

        public void ExchangeAF()
        {
            Swap(0);
        }

        public void ExchangeBCDEHL()
        {
            Swap(2); // BC
            Swap(4); // DE
            Swap(6); // HL
        }

        public Registers Snapshot()
        {
            return new Registers((byte[])_registers.Clone());
        }

        public void Clear()
        {
            _registers = new byte[26];
            WZ = 0x00;
        }

        private void Swap(int offset)
        {
            ushort value = Get16BitValue(offset);
            Set16BitValue(offset, Get16BitValue(offset + 8));
            Set16BitValue(offset + 8, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private byte GetRegister(ByteRegister register)
        {
            if (register == ByteRegister.None) return 0xFF;
            return _registers[(int)register];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ushort GetRegisterPair(WordRegister registerPair)
        {
            if (registerPair == WordRegister.None) return 0xFF;
            return Get16BitValue((int)registerPair);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetRegister(ByteRegister register, byte value)
        {
            if (register != ByteRegister.None)
            {
                _registers[(int)register] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetRegisterPair(WordRegister registerPair, ushort value)
        {
            if (registerPair != WordRegister.None)
            {
                Set16BitValue((int)registerPair, value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ushort Get16BitValue(int offset)
        {
            return (ushort)((_registers[offset] * 256) + _registers[offset + 1]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set16BitValue(int offset, ushort value)
        {
            _registers[offset] = (byte)(value / 256);
            _registers[offset + 1] = (byte)(value % 256);
        }

        public Registers()
        {
            _registers = new byte[26];
        }

        private Registers(byte[] registerValues) : this()
        {
            if (registerValues.Length != 26)
            {
                throw new ArgumentOutOfRangeException("Invalid format. Size of array registerValues must be 26 bytes.");
            }

            _registers = registerValues;
        }
    }
}

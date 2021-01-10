using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Zem80.Core
{
    public class Registers : IDebugRegisters
    {
        private byte[] _registers;

        private byte _accumulator;
        private byte _altAccumulator;
        private Flags _flags;
        private Flags _altFlags;

        public byte this[ByteRegister register] { get { return GetRegister(register); } set { SetRegister(register, value); } }
        public ushort this[WordRegister registerPair] { get { return GetRegisterPair(registerPair); } set { SetRegisterPair(registerPair, value); } }

        public IDebugRegisters Debug => this;

        // 8-bit registers
        public byte B { get { return _registers[0]; } set { _registers[0] = value; } }
        public byte C { get { return _registers[1]; } set { _registers[1] = value; } }
        public byte D { get { return _registers[2]; } set { _registers[2] = value; } }
        public byte E { get { return _registers[3]; } set { _registers[3] = value; } }
        public byte H { get { return _registers[4]; } set { _registers[4] = value; } }
        public byte L { get { return _registers[5]; } set { _registers[5] = value; } }
        public byte A { get { return _accumulator; } set { _accumulator = value; } }
        public byte F { get { return _flags.Value; } } // flags register - shouldn't set F directly (but you can via the Debug interface)

        byte IDebugRegisters.F { set { _flags = new Flags(value); } }

        // Registers as 16-bit pairs
        public ushort BC { get { return Get16BitValue(0); } set { Set16BitValue(0, value); } }
        public ushort DE { get { return Get16BitValue(2); } set { Set16BitValue(2, value); } }
        public ushort HL { get { return Get16BitValue(4); } set { Set16BitValue(4, value); } }
        public ushort AF { get { return GetWord(_accumulator, _flags.Value); } }
       
        ushort IDebugRegisters.AF { set { _accumulator = value.HighByte(); _flags = new Flags(value.LowByte()); } }

        // There is a second 'shadow' bank of register values (AF', BC', DE', HL'). These are stored in _registers[6..13] (and in private fields for AF/AF').
        // To access these you call ExchangeAF (to get access to values in AF') or ExchangeBCDEHL (to get access to values in BC', DE' and HL'). But for debug purposes we can
        // access them directly

        ushort IDebugRegisters.BC_ { get { return Get16BitValue(8); } set { Set16BitValue(8, value); } }
        ushort IDebugRegisters.DE_ { get { return Get16BitValue(10); } set { Set16BitValue(10, value); } }
        ushort IDebugRegisters.HL_ { get { return Get16BitValue(12); } set { Set16BitValue(12, value); } }
        ushort IDebugRegisters.AF_ { get { return GetWord(_altAccumulator, _altFlags.Value); } set { _altAccumulator = value.HighByte(); _altFlags = new Flags(value.LowByte()); } }

        // 16-bit special registers (index, stack pointer)
        public ushort IX { get { return Get16BitValue(14); } set { Set16BitValue(14, value); } }
        public ushort IY { get { return Get16BitValue(16); } set { Set16BitValue(16, value); } }
        public ushort SP { get { return Get16BitValue(18); } set { Set16BitValue(18, value); } }

        // high/low bytes of IX/IY
        public byte IXh { get { return _registers[14]; } set { _registers[14] = value; } }
        public byte IXl { get { return _registers[15]; } set { _registers[15] = value; } }
        public byte IYh { get { return _registers[16]; } set { _registers[16] = value; } }
        public byte IYl { get { return _registers[17]; } set { _registers[17] = value; } }

        // 8-bit 'other' registers
        public byte I { get { return _registers[20]; } set { _registers[20] = value; } }
        public byte R { get { return _registers[21]; } set { _registers[21] = value; } }

        // I+R as a 16-bit pair
        public ushort IR { get { return Get16BitValue(20); } }

        // program counter
        public ushort PC { get { return Get16BitValue(22); } set { Set16BitValue(22, value); } }

        internal ushort WZ { get; set; } // internal register, never exposed to the outside world and not saved / restored with snapshot

        public Flags Flags => _flags;

        public void ExchangeAF()
        {
            lock (this)
            {
                // The reason for storing A & F in fields rather than in the byte array is because we need to store the flags
                // as a Flags instance and not as a byte value, because the constant conversion back and forth
                // between byte and Flags is expensive and slows the emulation down significantly.
                // Swapping these values only involves swapping references which is as quick as changing the offset
                // for the other registers.

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
            Swap(0);
            Swap(2);
            Swap(4);

            void Swap(int offset)
            {
                ushort value = Get16BitValue(offset);
                Set16BitValue(offset, Get16BitValue(offset + 8));
                Set16BitValue(offset + 8, value);
            }
        }

        public Registers Snapshot()
        {
            return new Registers((byte[])_registers.Clone(), _accumulator, _altAccumulator, new Flags(_flags.Value), new Flags(_altFlags.Value));
        }

        public void Clear()
        {
            _registers = new byte[26];
            _flags = new Flags();
            _altFlags = new Flags();
            _accumulator = 0x00;
            _altAccumulator = 0x00;
            WZ = 0x00;
        }

        internal void SetFlags(byte newFlagsValue)
        {
            _flags = new Flags(newFlagsValue);
        }

        private byte GetRegister(ByteRegister register)
        {
            if (register == ByteRegister.None) return 0xFF;

            if (register == ByteRegister.A)
            {
                return _accumulator;
            }
            else 
            {
                return _registers[(int)register];
            }
        }

        private ushort GetRegisterPair(WordRegister registerPair)
        {
            if (registerPair == WordRegister.None) return 0xFF;

            if (registerPair == WordRegister.AF)
            {
                return GetWord(_accumulator, _flags.Value);
            }
            else 
            {
                return Get16BitValue((int)registerPair);
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
                    _registers[(int)register] = value;
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
                    _flags = new Flags(value.LowByte());
                }
                else
                {
                    Set16BitValue((int)registerPair, value);
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
            _registers = new byte[24];
            _flags = new Flags();
            _altFlags = new Flags();
        }

        private Registers(byte[] registerValues, byte accumulator, byte altAccumulator, Flags flags, Flags altFlags)
        {
            if (registerValues.Length != 24)
            {
                throw new ArgumentOutOfRangeException("Invalid format. Size of array registerValues must be 24 bytes.");
            }

            _registers = registerValues;
            _accumulator = accumulator;
            _altAccumulator = altAccumulator;
            _flags = new Flags(flags.Value);
            _altFlags = new Flags(altFlags.Value);
        }
    }
}

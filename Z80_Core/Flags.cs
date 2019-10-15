using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class Flags
    {
        private IRegisters _registers;

        public bool Sign { get { return Get(7); } set { Set(7, value); } }
        public bool Zero { get { return Get(6); } set { Set(6, value); } }
        public bool Five { get { return Get(5); } set { Set(5, value); } }
        public bool HalfCarry { get { return Get(4); } set { Set(4, value); } }
        public bool Three { get { return Get(3); } set { Set(3, value); } }
        public bool Parity { get { return Get(2); } set { Set(2, value); } }
        public bool Subtract { get { return Get(1); } set { Set(1, value); } }
        public bool Carry { get { return Get(0); } set { Set(0, value); } }

        private bool Get(int bitIndex)
        {
            return (_registers.F & (1 << bitIndex)) != 0;
        }

        private void Set(int bitIndex, bool value)
        {
            int mask = 1 << bitIndex;
            _registers.F = (byte)(value ? _registers.F | mask : _registers.F & ~mask);
        }

        private void ClearAll()
        {
            _registers.F = 0;
        }

        public Flags(IRegisters registers)
        {
            _registers = registers;
        }
    }
}

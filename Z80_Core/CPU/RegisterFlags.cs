using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public class RegisterFlags : IFlags
    {
        private Registers _registers;

        public bool Sign { get { return GetBit(7); } set { SetBit(7, value); } }
        public bool Zero { get { return GetBit(6); } set { SetBit(6, value); } }
        public bool Five { get { return GetBit(5); } set { SetBit(5, value); } }
        public bool HalfCarry { get { return GetBit(4); } set { SetBit(4, value); } }
        public bool Three { get { return GetBit(3); } set { SetBit(3, value); } }
        public bool ParityOverflow { get { return GetBit(2); } set { SetBit(2, value); } }
        public bool Subtract { get { return GetBit(1); } set { SetBit(1, value); } }
        public bool Carry { get { return GetBit(0); } set { SetBit(0, value); } }

        public byte Value => _registers.F;

        public void SetFrom(IFlags flags)
        {
            Carry = flags.Carry;
            Five = flags.Five;
            HalfCarry = flags.HalfCarry;
            ParityOverflow = flags.ParityOverflow;
            Sign = flags.Sign;
            Subtract = flags.Subtract;
            Three = flags.Three;
            Zero = flags.Zero;
        }

        private bool GetBit(int bitIndex)
        {
            return (_registers.F & (1 << bitIndex)) != 0;
        }

        private void SetBit(int bitIndex, bool value)
        {
            int mask = 1 << bitIndex;
            _registers.F = (byte)(value ? _registers.F | mask : _registers.F & ~mask);
        }

        private void ClearAll()
        {
            _registers.F = 0;
        }

        public RegisterFlags(Registers registers)
        {
            _registers = registers;
        }
    }
}

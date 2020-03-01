using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public enum Condition
    {
        Z, NZ, C, NC, PO, PE, M, P
    }

    public enum Flag
    {
        Sign, Zero, Five, HalfCarry, Three, ParityOverflow, Subtract, Carry
    }

    public class Flags
    {
        private byte _flags;
        private Action<byte> _set_flags;
        private Func<byte> _get_flags;

        public bool Sign { get { return GetBit(7); } set { SetBit(7, value); } }
        public bool Zero { get { return GetBit(6); } set { SetBit(6, value); } }
        public bool Five { get { return GetBit(5); } set { SetBit(5, value); } }
        public bool HalfCarry { get { return GetBit(4); } set { SetBit(4, value); } }
        public bool Three { get { return GetBit(3); } set { SetBit(3, value); } }
        public bool ParityOverflow { get { return GetBit(2); } set { SetBit(2, value); } }
        public bool Subtract { get { return GetBit(1); } set { SetBit(1, value); } }
        public bool Carry { get { return GetBit(0); } set { SetBit(0, value); } }

        public virtual byte Value => _get_flags();

        public void Set(byte flags)
        {
            _set_flags(flags);
        }

        public void SetFromCondition(Condition condition)
        {
            switch (condition)
            {
                case Condition.Z: Zero = true; break;
                case Condition.NZ: Zero = false; break;
                case Condition.C: Carry = true; break;
                case Condition.NC: Carry = false; break;
                case Condition.PE: ParityOverflow = true; break;
                case Condition.PO: ParityOverflow = false; break;
                case Condition.M: Sign = true; break;
                case Condition.P: Sign = false; break;
            }
        }

        public bool SatisfyCondition(Condition condition)
        {
            return condition switch
            {
                Condition.Z => Zero,
                Condition.NZ => !Zero,
                Condition.C => Carry,
                Condition.NC => !Carry,
                Condition.PE => ParityOverflow,
                Condition.PO => !ParityOverflow,
                Condition.M => Sign,
                Condition.P => !Sign,
                _ => false
            };
        }

        public bool Check(bool? sign = null, bool? carry = null, bool? halfCarry = null, bool? parityOverflow = null, bool? subtract = null, bool? zero = null)
        {
            return (sign.HasValue ? Sign == sign : true &&
                    carry.HasValue ? Carry == carry : true &&
                    halfCarry.HasValue ? HalfCarry == halfCarry : true &&
                    parityOverflow.HasValue ? ParityOverflow == parityOverflow : true &&
                    subtract.HasValue ? Subtract == subtract : true &&
                    zero.HasValue ? Zero == zero : true);
        }

        public override bool Equals(object obj)
        {
            return obj switch
            {
                Flags f => f.Value == Value,
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected virtual bool GetBit(int bitIndex)
        {
            return (_get_flags() & (1 << bitIndex)) != 0;
        }

        protected virtual void SetBit(int bitIndex, bool value)
        {
            int mask = 1 << bitIndex;
            _set_flags((byte)(value ? _get_flags() | mask : _get_flags() & ~mask));
        }

        protected virtual void ClearAll()
        {
            _set_flags(0);
        }

        public Flags() : this(0)
        {
        }

        public Flags(byte flags)
        {
            _flags = flags;
            _get_flags = () => _flags;
            _set_flags = (value) => _flags = value;
        }

        public Flags(Func<byte> getter, Action<byte> setter)
        {
            _get_flags = getter;
            _set_flags = setter;
        }
    }
}

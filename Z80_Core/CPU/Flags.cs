using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core
{
    public enum Condition
    {
        Z, NZ, C, NC, PO, PE, M, P
    }

    [Flags]
    public enum FlagState
    {
        None = 0,
        Carry = 1,
        Subtract = 2,
        Three = 4,
        ParityOverflow = 8,
        Five = 16,
        HalfCarry = 32,
        Zero = 64,
        Sign = 128
    }

    public class Flags
    {
        private byte _flags;

        public bool Sign { get { return _flags.GetBit(7); } set { _flags = _flags.SetBit(7, value); } }
        public bool Zero { get { return _flags.GetBit(6); } set { _flags = _flags.SetBit(6, value); } }
        public bool Five { get { return _flags.GetBit(5); } set { _flags = _flags.SetBit(5, value); } }
        public bool HalfCarry { get { return _flags.GetBit(4); } set { _flags = _flags.SetBit(4, value); } }
        public bool Three { get { return _flags.GetBit(3); } set { _flags = _flags.SetBit(3, value); } }
        public bool ParityOverflow { get { return _flags.GetBit(2); } set { _flags = _flags.SetBit(2, value); } }
        public bool Subtract { get { return _flags.GetBit(1); } set { _flags = _flags.SetBit(1, value); } }
        public bool Carry { get { return _flags.GetBit(0); } set { _flags = _flags.SetBit(0, value); } }
        public virtual byte Value { get { return _flags; } set { _flags = value; } }
        
        public FlagState State => GetState();

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
                FlagState f => (int)f == Value,
                _ => false
            };
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected virtual void ClearAll()
        {
            _flags = 0;
        }

        private FlagState GetState()
        {
            FlagState state = FlagState.None;
            if (Sign) state = state | FlagState.Sign;
            if (Zero) state = state | FlagState.Zero;
            if (HalfCarry) state = state | FlagState.HalfCarry;
            if (ParityOverflow) state = state | FlagState.ParityOverflow;
            if (Subtract) state = state | FlagState.Subtract;
            if (Carry) state = state | FlagState.Carry;
            return state;
        }

        public Flags() : this(0)
        {
        }

        public Flags(byte flags)
        {
            _flags = flags;
        }
    }
}

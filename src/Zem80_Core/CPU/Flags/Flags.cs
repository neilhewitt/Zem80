using System.Collections.Generic;
using System.Text;
using Zem80.Core.Instructions;

namespace Zem80.Core
{

    public class Flags
    {
        private byte _flags;

        public bool Sign { get { return _flags.GetBit(7); } set { _flags = _flags.SetBit(7, value); } }
        public bool Zero { get { return _flags.GetBit(6); } set { _flags = _flags.SetBit(6, value); } }
        public bool Y { get { return _flags.GetBit(5); } set { _flags = _flags.SetBit(5, value); } }
        public bool HalfCarry { get { return _flags.GetBit(4); } set { _flags = _flags.SetBit(4, value); } }
        public bool X { get { return _flags.GetBit(3); } set { _flags = _flags.SetBit(3, value); } }
        public bool ParityOverflow { get { return _flags.GetBit(2); } set { _flags = _flags.SetBit(2, value); } }
        public bool Subtract { get { return _flags.GetBit(1); } set { _flags = _flags.SetBit(1, value); } }
        public bool Carry { get { return _flags.GetBit(0); } set { _flags = _flags.SetBit(0, value); } }
        
        public virtual byte Value { get { return _flags; } }

        public FlagState State => (FlagState)_flags;

        public void Reset()
        {
            _flags = 0;
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

        // this is used by NUnit, so ignore the '0 references' and DO NOT REMOVE!
        public override bool Equals(object obj)
        {
            return obj switch
            {
                Flags f => f.Value == Value,
                FlagState f => (int)f == Value,
                _ => false
            };
        }

        // ditto as above
        public override int GetHashCode()
        {
            return base.GetHashCode();
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

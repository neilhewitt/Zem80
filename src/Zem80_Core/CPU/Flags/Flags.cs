using System.Collections.Generic;
using System.Text;
using Zem80.Core.Instructions;

namespace Zem80.Core
{
    public class Flags
    {
        private byte _flagByte;
        
        protected virtual byte FlagByte { get { return _flagByte; } set { _flagByte = value; } }
        
        public bool Sign { get { return FlagByte.GetBit(7); } set { FlagByte = FlagByte.SetBit(7, value); } }
        public bool Zero { get { return FlagByte.GetBit(6); } set { FlagByte = FlagByte.SetBit(6, value); } }
        public bool Y { get { return FlagByte.GetBit(5); } set { FlagByte = FlagByte.SetBit(5, value); } }
        public bool HalfCarry { get { return FlagByte.GetBit(4); } set { FlagByte = FlagByte.SetBit(4, value); } }
        public bool X { get { return FlagByte.GetBit(3); } set { FlagByte = FlagByte.SetBit(3, value); } }
        public bool ParityOverflow { get { return FlagByte.GetBit(2); } set { FlagByte = FlagByte.SetBit(2, value); } }
        public bool Subtract { get { return FlagByte.GetBit(1); } set { FlagByte = FlagByte.SetBit(1, value); } }
        public bool Carry { get { return FlagByte.GetBit(0); } set { FlagByte = FlagByte.SetBit(0, value); } }
        
        public byte Value { get { return FlagByte; } }

        public FlagState State => (FlagState)FlagByte;

        public void Reset()
        {
            FlagByte = 0;
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

        public Flags()
        {
        }

        public Flags(byte flags)
        {
            FlagByte = flags;
        }
    }
}

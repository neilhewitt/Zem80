using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.Instructions;

namespace Zem80.Core.CPU
{
    public class Flags : IReadOnlyFlags
    {
        private byte _flagByte;

        public bool Sign { get { return _flagByte.GetBit(7); } set { CheckReadOnly(); _flagByte = _flagByte.SetBit(7, value); } }
        public bool Zero { get { return _flagByte.GetBit(6); } set { CheckReadOnly(); _flagByte = _flagByte.SetBit(6, value); } }
        public bool Y { get { return _flagByte.GetBit(5); } set { CheckReadOnly(); _flagByte = _flagByte.SetBit(5, value); } }
        public bool HalfCarry { get { return _flagByte.GetBit(4); } set { CheckReadOnly(); _flagByte = _flagByte.SetBit(4, value); } }
        public bool X { get { return _flagByte.GetBit(3); } set { CheckReadOnly(); _flagByte = _flagByte.SetBit(3, value); } }
        public bool ParityOverflow { get { return _flagByte.GetBit(2); } set { CheckReadOnly(); _flagByte = _flagByte.SetBit(2, value); } }
        public bool Subtract { get { return _flagByte.GetBit(1); } set { CheckReadOnly(); _flagByte = _flagByte.SetBit(1, value); } }
        public bool Carry { get { return _flagByte.GetBit(0); } set { CheckReadOnly(); _flagByte = _flagByte.SetBit(0, value); } }

        public byte Value { get { return _flagByte; } }

        public bool ReadOnly { get; private set; }

        public FlagState State => (FlagState)_flagByte;

        public void Reset()
        {
            _flagByte = 0;
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

        public Flags Clone()
        {
            return new Flags(_flagByte);
        }

        private void CheckReadOnly()
        {
            if (ReadOnly) throw new InvalidOperationException("This flags object is read-only and cannot be written to.");
        }

        public Flags()
        {
        }

        public Flags(byte flags, bool readOnly = false)
        {
            _flagByte = flags;
            ReadOnly = readOnly;
        }
    }
}

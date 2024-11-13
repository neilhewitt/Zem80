using System;
using System.Collections.Generic;
using System.Text;
using Zem80.Core.CPU;

namespace Zem80.Core.CPU
{
    public class Flags : IReadOnlyFlags
    {
        private byte _flagByte;

        public bool Sign { get { return (byte)(_flagByte & 0x80) > 0; } set { _flagByte = ReadOnly ? _flagByte : value ? (byte)(_flagByte | 0x80) : (byte)(_flagByte & ~0x80); } }
        public bool Zero { get { return (byte)(_flagByte & 0x40) > 0; } set { _flagByte = ReadOnly ? _flagByte : value ? (byte)(_flagByte | 0x40) : (byte)(_flagByte & ~0x40); } }
        public bool Y { get { return (byte)(_flagByte & 0x20) > 0; } set { _flagByte = ReadOnly ? _flagByte : value ? (byte)(_flagByte | 0x20) : (byte)(_flagByte & ~0x20); } }
        public bool HalfCarry { get { return (byte)(_flagByte & 0x10) > 0; } set { _flagByte = ReadOnly ? _flagByte : value ? (byte)(_flagByte | 0x10) : (byte)(_flagByte & ~0x10); } }
        public bool X { get { return (byte)(_flagByte & 0x08) > 0; } set { _flagByte = ReadOnly ? _flagByte : value ? (byte)(_flagByte | 0x08) : (byte)(_flagByte & ~0x08); } }
        public bool ParityOverflow { get { return (byte)(_flagByte & 0x04) > 0; } set { _flagByte = ReadOnly ? _flagByte : value ? (byte)(_flagByte | 0x04) : (byte)(_flagByte & ~0x04); } }
        public bool Subtract { get { return (byte)(_flagByte & 0x02) > 0; } set { _flagByte = ReadOnly ? _flagByte : value ? (byte)(_flagByte | 0x02) : (byte)(_flagByte & ~0x02); } }
        public bool Carry { get { return (byte)(_flagByte & 0x01) > 0; } set { _flagByte = ReadOnly ? _flagByte : value ? (byte)(_flagByte | 0x01) : (byte)(_flagByte & ~0x01); } }

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

        public Flags()
        {
        }

        public Flags(FlagState flags, bool readOnly = false)
        {
            _flagByte = (byte)flags;
            ReadOnly = readOnly;
        }

        public Flags(byte flags, bool readOnly = false)
        {
            _flagByte = flags;
            ReadOnly = readOnly;
        }
    }
}

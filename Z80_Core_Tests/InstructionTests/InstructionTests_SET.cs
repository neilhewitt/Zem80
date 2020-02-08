using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_SET : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegistersAndBits")]
        public void SET_r(RegisterIndex register, int bitIndex)
        {
            IFlags currentFlags = Flags;
            byte initialValue = RandomByte();
            byte setValue = initialValue.SetBit(bitIndex, true);

            Registers[register] = initialValue;
            Execute($"SET { bitIndex },{ register }", bitIndex: (byte)bitIndex, registerIndex: register);

            Assert.That(Registers[register] == setValue && Flags.Equals(currentFlags)); // flags should be preserved
        }

        [Test]
        public void SET_xHL([Range(0, 6)] byte bitIndex)
        {
            IFlags currentFlags = Flags;
            byte initialValue = RandomByte();
            byte setValue = initialValue.SetBit(bitIndex, true);
            Registers.HL = RandomWord();
            WriteByteAt(Registers.HL, initialValue);

            Execute($"SET { bitIndex },(HL)", bitIndex: (byte)bitIndex);


            Assert.That(ByteAt(Registers.HL) == setValue && Flags.Equals(currentFlags)); // flags should be preserved
        }

        [Test]
        public void SET_xIndexOffset([Values(RegisterPairIndex.IX, RegisterPairIndex.IY)] RegisterPairIndex registerPair, [Range(0, 6)] byte bitIndex)
        {
            IFlags currentFlags = Flags;
            byte initialValue = RandomByte();
            byte setValue = initialValue.SetBit(bitIndex, true);
            ushort address = RandomWord();
            sbyte offset = (sbyte)RandomByte();

            Registers[registerPair] = address;
            WriteByteAt((ushort)(address + offset), initialValue);

            Execute($"SET { bitIndex },({ registerPair }+o)", bitIndex: (byte)bitIndex, arg1: (byte)offset);

            Assert.That(ByteAt((ushort)(address + offset)) == setValue && Flags.Equals(currentFlags)); // flags should be preserved
        }
    }
}
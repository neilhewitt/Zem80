using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RES : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegistersAndBits")]
        public void RES_r(Register register, int bitIndex)
        {
            IFlags currentFlags = Flags;
            byte initialValue = RandomByte();
            byte resetValue = initialValue.SetBit(bitIndex, false);
            
            Registers[register] = initialValue;
            Execute($"RES { bitIndex },{ register }", bitIndex:(byte)bitIndex, registerIndex:register);

            Assert.That(Registers[register] == resetValue && Flags.Equals(currentFlags)); // flags should be preserved
        }

        [Test]
        public void RES_xHL([Range(0, 6)] byte bitIndex)
        {
            IFlags currentFlags = Flags;
            byte initialValue = RandomByte();
            byte resetValue = initialValue.SetBit(bitIndex, false);
            Registers.HL = RandomWord();
            WriteByteAt(Registers.HL, initialValue);

            Execute($"RES { bitIndex },(HL)", bitIndex: (byte)bitIndex);


            Assert.That(ByteAt(Registers.HL) == resetValue && Flags.Equals(currentFlags)); // flags should be preserved
        }

        [Test]
        public void RES_xIndexOffset([Values(RegisterPair.IX, RegisterPair.IY)] RegisterPair registerPair, [Range(0, 6)] byte bitIndex)
        {
            IFlags currentFlags = Flags;
            byte initialValue = RandomByte();
            byte resetValue = initialValue.SetBit(bitIndex, false);
            ushort address = RandomWord();
            sbyte offset = (sbyte)RandomByte();

            Registers[registerPair] = address;
            WriteByteAt((ushort)(address + offset), initialValue);

            Execute($"RES { bitIndex },({ registerPair }+o)", bitIndex:(byte)bitIndex, arg1:(byte)offset);

            Assert.That(ByteAt((ushort)(address + offset)) == resetValue && Flags.Equals(currentFlags)); // flags should be preserved
        }
    }
}
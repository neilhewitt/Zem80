using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_BIT : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void BIT_b_r(RegisterIndex register)
        {
            byte bitIndex = RandomByte(7);
            byte value = RandomByte();
            bool zero = value.GetBit(bitIndex) == false;

            Registers[register] = value;
            Execute($"BIT {bitIndex},{register}", registerIndex: register);

            Assert.That(TestFlags(zero: zero, halfCarry: true));
        }

        [Test]
        public void BIT_b_xHL()
        {
            byte bitIndex = RandomByte(7);
            byte value = RandomByte();
            ushort address = RandomWord();
            bool zero = value.GetBit(bitIndex) == false;

            WriteByteAt(address, value);
            Registers.HL = address;
            Execute($"BIT {bitIndex},(HL)");

            Assert.That(TestFlags(zero: zero, halfCarry: true));
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void BIT_b_xIndexOffset(RegisterPairIndex registerPair)
        {
            byte bitIndex = RandomByte(7);
            byte value = RandomByte();
            sbyte offset = (sbyte)RandomByte();
            ushort address = RandomWord();
            bool zero = value.GetBit(bitIndex) == false;

            WriteByteAt((ushort)(address + offset), value);
            Registers[registerPair] = address;
            Execute($"BIT {bitIndex},(HL)", arg1:(byte)offset);

            Assert.That(TestFlags(zero: zero, halfCarry: true));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_NEG : InstructionTestBase
    {
        [Test]
        public void NEG()
        {
            byte value = RandomByte();
            Registers.A = value;
            Execute("NEG");

            sbyte actual = (sbyte)Registers.A;
            sbyte result = (sbyte)(0 - value);
            bool zero = result == 0;
            bool sign = result < 0;
            bool halfCarry = result.HalfCarryWhenConvertingToByte();
            bool parityOverflow = (byte)result == 0x80;
            bool carry = (byte)result != 0x00;

            Assert.That(actual == result && TestFlags(zero: zero, sign: sign, halfCarry: halfCarry, parityOverflow: parityOverflow, carry: carry));
        }
    }
}
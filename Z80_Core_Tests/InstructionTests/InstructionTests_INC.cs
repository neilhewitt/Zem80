using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_INC : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void INC_r(RegisterIndex register)
        {
            bool carry = RandomByte() % 2 == 0; // simulate existing carry
            Registers.Flags.Carry = carry;

            byte value = RandomByte();
            ushort result = (ushort)(value + 1);
            bool zero = result == 0;
            bool sign = (sbyte)result < 0;
            bool halfCarry = (result & 0x0F) == 0x0F;
            bool overflow = result == 0x7F;

            Registers[register] = value;
            Execute($"INC {register}");

            Assert.That(Registers[register] == (byte)result &&
                TestFlags(carry: carry, zero: zero, sign: sign, halfCarry: halfCarry, parityOverflow: overflow, subtract: true));
        }
    }
}
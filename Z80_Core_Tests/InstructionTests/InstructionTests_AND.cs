using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_AND : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void AND_r(Register register)
        {
            byte first = RandomByte();
            byte second = register == Register.A ? first : RandomByte(); // if AND A, use first byte only
            byte result = (byte)(first & second); 

            bool sign = ((sbyte)result < 0);
            bool zero = result == 0;
            bool overflow = ((sbyte)result > 0x7F || (sbyte)result < -0x80);

            Registers.A = first;
            Registers[register] = second;

            Execute($"AND {register}", registerIndex: register);

            Assert.That(Registers.A == result && TestFlags(sign: sign, zero: zero, parityOverflow: overflow, halfCarry: true));
        }

        [Test]
        public void AND_n()
        {
            byte first = RandomByte();
            byte second = RandomByte();
            byte result = (byte)(first & second);

            bool sign = ((sbyte)result < 0);
            bool zero = result == 0;
            bool overflow = ((sbyte)result > 0x7F || (sbyte)result < -0x80);

            Registers.A = first;

            Execute($"AND n", arg1: second);

            Assert.That(Registers.A == result && TestFlags(sign: sign, zero: zero, parityOverflow: overflow, halfCarry: true));
        }

        [Test]
        public void AND_xHL()
        {
            byte first = RandomByte();
            byte second = RandomByte();
            ushort address = RandomWord();
            byte result = (byte)(first & second);

            bool sign = ((sbyte)result < 0);
            bool zero = result == 0;
            bool overflow = ((sbyte)result > 0x7F || (sbyte)result < -0x80);

            Registers.A = first;
            Registers.HL = address;
            WriteByteAt(Registers.HL, second);

            Execute($"AND (HL)");

            Assert.That(Registers.A == result && TestFlags(sign: sign, zero: zero, parityOverflow: overflow, halfCarry: true));
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void AND_xIndexOffset(RegisterPair registerPair)
        {
            byte first = RandomByte();
            byte second = RandomByte();
            sbyte offset = (sbyte)RandomByte();
            ushort address = RandomWord(0x8888);
            byte result = (byte)(first & second);

            bool sign = ((sbyte)result < 0);
            bool zero = result == 0;
            bool overflow = ((sbyte)result > 0x7F || (sbyte)result < -0x80);

            Registers.A = first;
            Registers[registerPair] = address;
            WriteByteAt((ushort)(Registers[registerPair] + offset), second);

            Execute($"AND ({registerPair}+o)", arg1:(byte)offset);

            Assert.That(Registers.A == result && TestFlags(sign: sign, zero: zero, parityOverflow: overflow, halfCarry: true));
        }
    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_OR : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        [TestCase(Register.IXh)]
        [TestCase(Register.IXl)]
        [TestCase(Register.IYh)]
        [TestCase(Register.IYl)]
        public void OR_r(Register register)
        {
            byte first = RandomByte();
            byte second = (register == Register.A) ? first : RandomByte();
            short result = (short)(first | second);

            bool zero = (result == 0);
            bool sign = ((sbyte)result < 0);
            bool parityOverflow = (result > 0xFF);

            Registers.A = first;
            Registers[register] = second;
            Execute($"OR {register}");

            Assert.That(Registers.A == (byte)result && 
                TestFlags(zero: zero, sign: sign, halfCarry: false, parityOverflow: parityOverflow, carry: false, subtract: false));
        }

        [Test]
        public void OR_n()
        {
            byte first = RandomByte();
            byte second = RandomByte();
            short result = (short)(first | second);

            bool zero = (result == 0);
            bool sign = ((sbyte)result < 0);
            bool parityOverflow = (result > 0xFF);

            Registers.A = first;
            Execute($"OR n", arg1: second);

            Assert.That(Registers.A == (byte)result &&
                TestFlags(zero: zero, sign: sign, halfCarry: false, parityOverflow: parityOverflow, carry: false, subtract: false));
        }

        [Test]
        public void OR_xHL()
        {
            byte first = RandomByte();
            byte second = RandomByte();
            ushort address = RandomWord();
            short result = (short)(first | second);

            bool zero = (result == 0);
            bool sign = ((sbyte)result < 0);
            bool parityOverflow = (result > 0xFF);

            Registers.A = first;
            Registers.HL = address;
            WriteByteAt(Registers.HL, second);
            Execute($"OR (HL)");

            Assert.That(Registers.A == (byte)result &&
                TestFlags(zero: zero, sign: sign, halfCarry: false, parityOverflow: parityOverflow, carry: false, subtract: false));
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void OR_xIndexOffset(RegisterPair registerPair)
        {
            byte first = RandomByte();
            byte second = RandomByte();
            byte offset = RandomByte(0x7F);
            ushort address = RandomWord();
            short result = (short)(first | second);

            bool zero = (result == 0);
            bool sign = ((sbyte)result < 0);
            bool parityOverflow = (result > 0xFF);

            Registers.A = first;
            Registers[registerPair] = address;
            WriteByteAt((ushort)(address + offset), second);
            Execute($"OR ({registerPair}+o)", arg1: offset);

            Assert.That(Registers.A == (byte)result &&
                TestFlags(zero: zero, sign: sign, halfCarry: false, parityOverflow: parityOverflow, carry: false, subtract: false));
        }
    }
}

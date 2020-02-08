using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_CP : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void CP_r(Register register)
        {
            byte first = RandomByte();
            byte second = (register == Register.A) ? first : RandomByte();
            short result = (short)(first - second);
            
            bool zero = (result == 0);
            bool sign = ((sbyte)result < 0);
            bool halfCarry = first.HalfCarryWhenSubtracting(second);
            bool parityOverflow = first.OverflowsWhenSubtracting(second);
            bool carry = (result > 0xFF);

            Registers.A = first;
            Registers[register] = second;
            Execute($"CP {register}");

            Assert.That(TestFlags(zero: zero, sign: sign, halfCarry: halfCarry, parityOverflow: parityOverflow, carry: carry));
        }

        [Test]
        public void CP_n()
        {
            byte first = RandomByte();
            byte second = RandomByte();
            short result = (short)(first - second);

            bool zero = (result == 0);
            bool sign = ((sbyte)result < 0);
            bool halfCarry = first.HalfCarryWhenSubtracting(second);
            bool parityOverflow = first.OverflowsWhenSubtracting(second);
            bool carry = (result > 0xFF);

            Registers.A = first;
            Execute($"CP n", arg1:second);

            Assert.That(TestFlags(zero: zero, sign: sign, halfCarry: halfCarry, parityOverflow: parityOverflow, carry: carry));
        }

        [Test]
        public void CP_xHL()
        {
            byte first = RandomByte();
            byte second = RandomByte();
            ushort address = RandomWord();
            short result = (short)(first - second);

            bool zero = (result == 0);
            bool sign = ((sbyte)result < 0);
            bool halfCarry = first.HalfCarryWhenSubtracting(second);
            bool parityOverflow = first.OverflowsWhenSubtracting(second);
            bool carry = (result > 0xFF);

            Registers.A = first;
            Registers.HL = address;
            WriteByteAt(Registers.HL, second);
            Execute($"CP (HL)");

            Assert.That(TestFlags(zero: zero, sign: sign, halfCarry: halfCarry, parityOverflow: parityOverflow, carry: carry));
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void CP_xIndexOffset(RegisterPair registerPair)
        {
            byte first = RandomByte();
            byte second = RandomByte();
            byte offset = RandomByte(0x7F);
            ushort address = RandomWord();
            short result = (short)(first - second);

            bool zero = (result == 0);
            bool sign = ((sbyte)result < 0);
            bool halfCarry = first.HalfCarryWhenSubtracting(second);
            bool parityOverflow = first.OverflowsWhenSubtracting(second);
            bool carry = (result > 0xFF);

            Registers.A = first;
            Registers[registerPair] = address;
            WriteByteAt((ushort)(address + offset), second);
            Execute($"CP ({registerPair}+o)", arg1:offset);

            Assert.That(TestFlags(zero: zero, sign: sign, halfCarry: halfCarry, parityOverflow: parityOverflow, carry: carry));
        }
    }
}

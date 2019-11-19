using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_DEC : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        [TestCase(RegisterIndex.IXh)]
        [TestCase(RegisterIndex.IXl)]
        public void DEC_r(RegisterIndex register)
        {
            byte value = RandomByte();
            Registers[register] = value;
            bool carry = RandomByte() > 0x7F;
            Registers.Flags.Carry = carry;

            Execute($"DEC {register}");
            byte result = Registers[register];

            Assert.That(Registers[register] == value - 1 &&
                TestFlags(
                    carry: carry,
                    zero: (result == 0),
                    sign: (((sbyte)result) < 0),
                    halfCarry: ((result & 0x0F) == 0),
                    parityOverflow: (value == 0x80),
                    subtract: true
                ));
        }

        [Test, TestCaseSource(typeof(TestCases), "GetRegisterPairs")]
        public void DEC_rr(RegisterPairIndex registerPair)
        {
            ushort value = RandomWord();
            Registers[registerPair] = value;

            Execute($"DEC {registerPair}");
            ushort result = Registers[registerPair];

            Assert.That(Registers[registerPair] == value - 1); // no flags affected (odd, eh?)
        }

        [Test]
        public void DEC_xHL()
        {
            ushort address = RandomWord();
            byte value = RandomByte();
            bool carry = RandomByte() > 0x7F;
            Registers.Flags.Carry = carry;

            Registers.HL = address;
            WriteByteAt(address, value);

            Execute("DEC (HL)");
            byte result = ByteAt(address);

            Assert.That(result == value - 1 && TestFlags(
                    carry: carry,
                    zero: (result == 0),
                    sign: (((sbyte)result) < 0),
                    halfCarry: ((result & 0x0F) == 0),
                    parityOverflow: (value == 0x80),
                    subtract: true
                ));
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void DEC_xIndexOffset(RegisterPairIndex indexRegister)
        {
            ushort address = RandomWord();
            byte value = RandomByte();
            sbyte offset = (sbyte)RandomByte();
            bool carry = RandomByte() > 0x7F;
            Registers.Flags.Carry = carry;

            Registers[indexRegister] = address;
            WriteByteAt((ushort)(address + offset), value);

            Execute($"DEC ({indexRegister}+o)", arg1:(byte)offset);
            byte result = ByteAt((ushort)(address + offset));

            Assert.That(result == value - 1 && TestFlags(
                    carry: carry,
                    zero: (result == 0),
                    sign: (((sbyte)result) < 0),
                    halfCarry: ((result & 0x0F) == 0),
                    parityOverflow: (value == 0x80),
                    subtract: true
                ));
        }
    }
}
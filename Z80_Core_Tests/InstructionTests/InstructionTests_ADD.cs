using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_ADD : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void ADD_r_r(Register register)
        {
            byte initialValue = RandomByte();
            byte addValue = register == Register.A ? initialValue : RandomByte(); // if ADD A,A have to have the same two numbers
            ushort addedValue = (ushort)(initialValue + addValue);
            bool flagCarry = addedValue > 255;
            bool flagOverflow = ((short)addedValue > 0x7F || (short)addedValue < -0x80);
            bool flagZero = addedValue == 0;
            bool flagSign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;
            Registers[register] = addValue;

            var result = Execute($"ADD A,{register}", registerIndex: register);

            Assert.That(Registers.A == (byte)addedValue && TestFlags(carry: flagCarry, zero: flagZero, parityOverflow: flagOverflow, sign: flagSign));
        }

        [Test]
        public void ADD_A_n()
        {
            byte initialValue = RandomByte();
            byte addValue = RandomByte();
            ushort addedValue = (ushort)(initialValue + addValue);
            bool flagCarry = addedValue > 255;
            bool flagOverflow = ((short)addedValue > 0x7F || (short)addedValue < -0x80);
            bool flagZero = addedValue == 0;
            bool flagSign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;

            var result = Execute($"ADD A,n", arg1: addValue);

            Assert.That(Registers.A == (byte)addedValue && TestFlags(carry: flagCarry, zero: flagZero, parityOverflow: flagOverflow, sign: flagSign));
        }

        [Test]
        public void ADD_A_xHL()
        {
            byte initialValue = RandomByte();
            byte addValue = RandomByte();
            ushort address = RandomWord();
            ushort addedValue = (ushort)(initialValue + addValue);
            bool flagCarry = addedValue > 255;
            bool flagOverflow = ((short)addedValue > 0x7F || (short)addedValue < -0x80);
            bool flagZero = addedValue == 0;
            bool flagSign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;
            WriteByteAt(address, addValue);
            Registers.HL = address;

            var result = Execute($"ADD A,(HL)");

            Assert.That(Registers.A == (byte)addedValue && TestFlags(carry: flagCarry, zero: flagZero, parityOverflow: flagOverflow, sign: flagSign));
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void ADD_A_xIndexOffset(RegisterPair registerPair)
        {
            byte offset = 139;// RandomByte();
            byte initialValue = 207;// RandomByte();
            byte addValue = 146;// RandomByte();
            ushort address = RandomWord(0xF700);
            ushort addedValue = (ushort)(initialValue + addValue);
            bool flagCarry = addedValue > 255;
            bool flagOverflow = ((short)addedValue > 0x7F || (short)addedValue < -0x80);
            bool flagZero = addedValue == 0;
            bool flagSign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;
            WriteByteAt((ushort)(address + (sbyte)offset), addValue);
            Registers[registerPair] = address;

            var result = Execute($"ADD A,({registerPair}+o)", arg1: offset);

            Assert.That(Registers.A == (byte)addedValue && TestFlags(carry: flagCarry, zero: flagZero, parityOverflow: flagOverflow, sign: flagSign));
        }
    }
}

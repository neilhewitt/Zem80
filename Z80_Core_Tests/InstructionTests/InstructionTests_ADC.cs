using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_ADC : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void ADC_r_r(Register register)
        {
            byte initialValue = RandomByte();
            byte addValue = register == Register.A ? initialValue : RandomByte(); // if ADC A,A have to have the same two numbers
            PresetFlags(carry: RandomByte() > 127); // simulate prior carry flag state which must be preserved
            ushort addedValue = (ushort)(initialValue + addValue + (Flags.Carry ? 1 : 0));
            bool flagCarry = addedValue > 255;
            bool flagOverflow = ((sbyte)addedValue > 0x7F || (sbyte)addedValue < -0x80);
            bool flagZero = addedValue == 0;
            bool flagSign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;
            Registers[register] = addValue;

            var result = Execute($"ADC A,{register}", registerIndex: register);

            Assert.That(Registers.A == (byte)addedValue && TestFlags(carry: flagCarry, zero: flagZero, parityOverflow: flagOverflow, sign: flagSign));
        }

        [Test]
        public void ADC_A_n()
        {
            byte initialValue = RandomByte();
            byte addValue = RandomByte();
            PresetFlags(carry: RandomByte() > 127);
            ushort addedValue = (ushort)(initialValue + addValue + (Flags.Carry ? 1 : 0));
            bool flagCarry = addedValue > 255;
            bool flagOverflow = ((sbyte)addedValue > 0x7F || (sbyte)addedValue < -0x80);
            bool flagZero = addedValue == 0;
            bool flagSign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;

            var result = Execute($"ADC A,n", arg1:addValue);

            Assert.That(Registers.A == (byte)addedValue && TestFlags(carry: flagCarry, zero: flagZero, parityOverflow: flagOverflow, sign: flagSign));
        }

        [Test]
        public void ADC_A_xHL()
        {
            byte initialValue = RandomByte();
            byte addValue = RandomByte();
            ushort address = RandomWord();
            PresetFlags(carry: RandomByte() > 127);
            ushort addedValue = (ushort)(initialValue + addValue + (Flags.Carry ? 1 : 0));
            bool flagCarry = addedValue > 255;
            bool flagOverflow = ((sbyte)addedValue > 0x7F || (sbyte)addedValue < -0x80);
            bool flagZero = addedValue == 0;
            bool flagSign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;
            WriteByteAt(address, addValue);
            Registers.HL = address;

            var result = Execute($"ADC A,(HL)");

            Assert.That(Registers.A == (byte)addedValue && TestFlags(carry: flagCarry, zero: flagZero, parityOverflow: flagOverflow, sign: flagSign));
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void ADC_A_xIndexOffset(RegisterPair registerPair)
        {
            byte offset = RandomByte();
            byte initialValue = RandomByte();
            byte addValue = RandomByte();
            ushort address = RandomWord(0xF700);
            PresetFlags(carry: RandomByte() > 127);
            ushort addedValue = (ushort)(initialValue + addValue + (Flags.Carry ? 1 : 0));
            bool flagCarry = addedValue > 255;
            bool flagOverflow = ((sbyte)addedValue > 0x7F || (sbyte)addedValue < -0x80);
            bool flagZero = addedValue == 0;
            bool flagSign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;
            WriteByteAt((ushort)(address + (sbyte)offset), addValue);
            Registers[registerPair] = address;

            var result = Execute($"ADC A,({registerPair}+o)", arg1:offset);

            Assert.That(Registers.A == (byte)addedValue && TestFlags(carry: flagCarry, zero: flagZero, parityOverflow: flagOverflow, sign: flagSign));
        }
    }
}

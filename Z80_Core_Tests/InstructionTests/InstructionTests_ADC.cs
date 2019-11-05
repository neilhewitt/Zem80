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
        public void ADC_r_r(RegisterIndex register)
        {
            byte initialValue = RandomByte();
            byte addValue = register == RegisterIndex.A ? initialValue : RandomByte(); // if ADC A,A have to have the same two numbers
            ushort addedValue = (ushort)(initialValue + addValue);
            bool carry = addedValue > 255;
            bool zero = addedValue == 0;
            bool sign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;
            Registers[register] = addValue;

            var result = Execute($"ADC A,{register}", registerIndex: register);

            Assert.That(Registers.A == (byte)addedValue && Registers.Flags.Carry == carry && Registers.Flags.Zero == zero && Registers.Flags.Sign == sign);
        }

        [Test]
        public void ADC_A_n()
        {
            byte initialValue = RandomByte();
            byte addValue = RandomByte();
            ushort addedValue = (ushort)(initialValue + addValue);
            bool carry = addedValue > 255;
            bool zero = addedValue == 0;
            bool sign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;

            var result = Execute($"ADC A,n", arg1:addValue);

            Assert.That(Registers.A == (byte)addedValue && Registers.Flags.Carry == carry && Registers.Flags.Zero == zero && Registers.Flags.Sign == sign);
        }

        [Test]
        public void ADC_A_xHL()
        {
            byte initialValue = RandomByte();
            byte addValue = RandomByte();
            ushort address = RandomWord();
            ushort addedValue = (ushort)(initialValue + addValue);
            bool carry = addedValue > 255;
            bool zero = addedValue == 0;
            bool sign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;
            WriteByteAt(address, addValue);
            Registers.HL = address;

            var result = Execute($"ADC A,(HL)");

            Assert.That(Registers.A == (byte)addedValue && Registers.Flags.Carry == carry && Registers.Flags.Zero == zero && Registers.Flags.Sign == sign);
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void ADC_A_xIndexOffset(RegisterPairIndex registerPair)
        {
            byte offset = RandomByte();
            byte initialValue = RandomByte();
            byte addValue = RandomByte();
            ushort address = RandomWord(0xF700);
            ushort addedValue = (ushort)(initialValue + addValue);
            bool carry = addedValue > 255;
            bool zero = addedValue == 0;
            bool sign = ((sbyte)addedValue < 0);

            Registers.A = initialValue;
            WriteByteAt((ushort)(address + offset), addValue);
            Registers[registerPair] = address;

            var result = Execute($"ADC A,({registerPair}+o)", arg1:offset);

            Assert.That(Registers.A == (byte)addedValue && Registers.Flags.Carry == carry && Registers.Flags.Zero == zero && Registers.Flags.Sign == sign);
        }
    }
}

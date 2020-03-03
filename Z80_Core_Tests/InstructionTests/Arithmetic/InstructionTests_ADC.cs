using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_ADC : InstructionTestBase
    {
        private (byte, Flags) GetExpectedResultAndFlags(byte value, byte add, bool carry)
        {
            Flags flags = new Flags();
            if (carry) add++;
            int result = value + add;
            flags = FlagLookup.FlagsFromArithmeticOperation(value, add, false);
            return ((byte)result, flags);
        }

        private (ushort, Flags) GetExpectedResultAndFlags(ushort value, ushort add, bool carry)
        {
            Flags flags = new Flags();
            if (carry) add++;
            int result = value + add;
            flags = FlagLookup.FlagsFromArithmeticOperation16Bit(flags, value, add, result, true, false);
            return ((ushort)result, flags);
        }

        [Test]
        public void ADC_A_r([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte add = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input; 
            Registers.B = add;

            ExecutionResult executionResult = ExecuteInstruction($"ADC A,B");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, add, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void ADC_A_n([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte add = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"ADC A,n", arg1: add);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, add, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void ADC_A_xHL([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte add = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, add);

            ExecutionResult executionResult = ExecuteInstruction($"ADC A,(HL)");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, add, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void ADC_A_xIndexOffset([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry, [Values(127,-127)] sbyte offset, 
            [Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair)
        {
            byte add = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers[registerPair] = 0x5000;
            WriteByteAt((ushort)(Registers[registerPair] + offset), add);

            ExecutionResult executionResult = ExecuteInstruction($"ADC A,({ registerPair }+o)", arg1: (byte)offset);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, add, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void ADC_HL_rr([Values(0x0000, 0x7FFE, 0x7FFF, 0xFFFF)] int input, [Values(true, false)] bool carry,
            [Values(RegisterPairName.BC, RegisterPairName.DE, RegisterPairName.SP)] RegisterPairName registerPair)
        {
            ushort add = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.HL = (ushort)input;
            Registers[registerPair] = add;

            ExecutionResult executionResult = ExecuteInstruction($"ADC HL,{ registerPair }");
            (ushort expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags((ushort)input, add, carry);

            Assert.That(Registers.HL, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }
    }
}

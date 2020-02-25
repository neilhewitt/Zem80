using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_ADD : InstructionTestBase
    {
        private (byte, Flags) GetExpectedResultAndFlags(byte value, byte add, bool carry)
        {
            Flags flags = new Flags();

            ushort result = (ushort)(value + add);
            short signedResult = (short)result;

            flags.Carry = result > 0xFF;
            flags.ParityOverflow = (signedResult > 0x7F || signedResult < -0x80);
            flags.Zero = result == 0;
            flags.Sign = ((sbyte)signedResult < 0);
            flags.HalfCarry = value.HalfCarryWhenAdding(add);

            return ((byte)result, flags);
        }


        [Test]
        public void ADD_A_r([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte add = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers.B = add;

            ExecutionResult executionResult = ExecuteInstruction($"ADD A,B");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, add, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void ADD_A_n([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte add = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"ADD A,n", arg1: add);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, add, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void ADD_A_xHL([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte add = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, add);

            ExecutionResult executionResult = ExecuteInstruction($"ADD A,(HL)");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, add, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void ADD_A_xIndexOffset([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry, [Values(127, -127)] sbyte offset,
            [Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair)
        {
            byte add = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers[registerPair] = 0x5000;
            WriteByteAt((ushort)(Registers[registerPair] + offset), add);

            ExecutionResult executionResult = ExecuteInstruction($"ADD A,({ registerPair }+o)", arg1: (byte)offset);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, add, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }
    }
}

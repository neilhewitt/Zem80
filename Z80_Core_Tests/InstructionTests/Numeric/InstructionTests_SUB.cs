using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_SUB : InstructionTestBase
    {
        private (byte, Flags) GetExpectedResultAndFlags(byte value, byte sub, bool carry)
        {
            Flags flags = new Flags();

            ushort result = (ushort)(value - sub);
            short signedResult = (short)result;

            flags.Carry = result < 0x00;
            flags.ParityOverflow = (signedResult > 0x7F || signedResult < -0x80);
            flags.Zero = result == 0;
            flags.Sign = ((sbyte)signedResult < 0);
            flags.HalfCarry = value.HalfCarryWhenSubtracting(sub);

            return ((byte)result, flags);
        }


        [Test]
        public void SUB_r([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte sub = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers.B = sub;

            ExecutionResult executionResult = ExecuteInstruction($"SUB B");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, sub, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SUB_n([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte sub = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"SUB n", arg1: sub);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, sub, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SUB_xHL([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte sub = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, sub);

            ExecutionResult executionResult = ExecuteInstruction($"SUB (HL)");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, sub, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SUB_xIndexOffset([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry, [Values(127, -127)] sbyte offset,
            [Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair)
        {
            byte sub = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers[registerPair] = 0x5000;
            WriteByteAt((ushort)(Registers[registerPair] + offset), sub);

            ExecutionResult executionResult = ExecuteInstruction($"SUB ({ registerPair }+o)", arg1: (byte)offset);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, sub, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }
    }
}

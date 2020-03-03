using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_SUB : InstructionTestBase
    {
        private (byte, Flags) GetExpectedResultAndFlags(byte value, byte sub)
        {
            Flags flags = new Flags();

            int result = value - sub;
            flags = FlagLookup.FlagsFromArithmeticOperation(value, sub, true);
            return ((byte)result, flags);
        }


        [Test]
        public void SUB_r([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input)
        {
            byte sub = 0x01; // with input range, covers all cases

            Registers.A = input;
            Registers.B = sub;

            ExecutionResult executionResult = ExecuteInstruction($"SUB B");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, sub);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SUB_n([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input)
        {
            byte sub = 0x01; // with input range, covers all cases

            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"SUB n", arg1: sub);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, sub);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SUB_xHL([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input)
        {
            byte sub = 0x01; // with input range, covers all cases

            Registers.A = input;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, sub);

            ExecutionResult executionResult = ExecuteInstruction($"SUB (HL)");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, sub);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SUB_xIndexOffset([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(127, -127)] sbyte offset,
            [Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair)
        {
            byte sub = 0x01; // with input range, covers all cases

            Registers.A = input;
            Registers[registerPair] = 0x5000;
            WriteByteAt((ushort)(Registers[registerPair] + offset), sub);

            ExecutionResult executionResult = ExecuteInstruction($"SUB ({ registerPair }+o)", arg1: (byte)offset);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, sub);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }
    }
}

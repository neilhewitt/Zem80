using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_AND : InstructionTestBase
    {
        private (byte result, Flags flags) GetExpectedResultAndFlags(byte first, byte second)
        {
            Flags flags = new Flags();
            byte result = (byte)(first & second);
            FlagLookup.FlagsFromLogicalOperation(first, second, LogicalOperation.And);
            return (result, flags);
        }

        [Test]
        public void AND_r([Values(0x00, 0x7F, 0x80, 0xFF)] byte first, [Values(0x7F, 0x80, 0xFF)] byte second)
        {
            Registers.A = first;
            Registers.B = second;

            ExecutionResult executionResult = ExecuteInstruction($"AND B");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(first, second);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void AND_n([Values(0x00, 0x7F, 0x80, 0xFF)] byte first, [Values(0x7F, 0x80, 0xFF)] byte second)
        {
            Registers.A = first;

            ExecutionResult executionResult = ExecuteInstruction($"AND n", arg1: second);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(first, second);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void AND_xHL([Values(0x00, 0x7F, 0x80, 0xFF)] byte first, [Values(0x7F, 0x80, 0xFF)] byte second)
        {
            Registers.A = first;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, second);

            ExecutionResult executionResult = ExecuteInstruction($"AND (HL)");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(first, second);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void AND_xIndexOffset([Values(0x00, 0x7F, 0x80, 0xFF)] byte first, [Values(0x7F, 0x80, 0xFF)] byte second,
            [Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair, [Values(127, -127)] sbyte offset)
        {
            Registers.A = first;
            Registers[registerPair] = 0x5000;
            WriteByteAt((ushort)(Registers[registerPair] + offset), second);

            ExecutionResult executionResult = ExecuteInstruction($"AND ({ registerPair }+o)", arg1: (byte)offset);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(first, second);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }
    }
}

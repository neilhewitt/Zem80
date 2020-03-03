using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_INC : InstructionTestBase
    {
        private (byte, Flags) GetExpectedResultAndFlags(byte input)
        {
            Flags flags = new Flags();
            int result = input + 1;
            flags = FlagLookup.FlagsFromArithmeticOperation(input, 1, true);
            return ((byte)result, flags);
        }

        [Test]
        public void INC_r([Values(0x7F, 0xFF, 0x01, 0x00)] byte input)
        {
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"INC A");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void INC_xHL([Values(0x7F, 0xFF, 0x01, 0x00)] byte input)
        {
            ushort address = 0x5000;
            Registers.HL = address;
            WriteByteAt(address, input);

            ExecutionResult executionResult = ExecuteInstruction($"INC (HL)");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input);

            Assert.That(ReadByteAt(address), Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void INC_xIndexOffset([Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair, [Values(127, -127)] sbyte offset,
            [Values(0x7F, 0xFF, 0x01, 0x00)] byte input)
        {
            ushort address = 0x5000;
            Registers[registerPair] = address;
            WriteByteAtIndexAndOffset(registerPair, offset, input); // write input to address pointed to by (<index register> + offset)

            ExecutionResult executionResult = ExecuteInstruction($"INC ({registerPair}+o)", arg1: (byte)offset);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input);

            Assert.That(ReadByteAtIndexAndOffset(registerPair, offset), Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }
    }
}
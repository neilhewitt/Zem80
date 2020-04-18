using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_DEC : InstructionTestBase
    {
        private (byte, Flags) GetExpectedResultAndFlags(byte input)
        {
            Flags flags = new Flags();
            sbyte subtract = -1;
            int result = input - 1;
            flags = FlagLookup.ByteArithmeticFlags(input, (byte)subtract, false, true);
            return ((byte)result, flags);
        }

        [Test]
        [TestCase(0x02, 0x01, FlagState.Subtract)]
        [TestCase(0x7F, 0x7E, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x10, 0x0F, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x81, 0x80, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        public void DEC_r(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"DEC A");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x02, 0x01, FlagState.Subtract)]
        [TestCase(0x7F, 0x7E, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x10, 0x0F, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x81, 0x80, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        public void DEC_xHL(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, input);

            ExecutionResult executionResult = ExecuteInstruction($"DEC (HL)");

            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedState));
        }


        [Test]
        [TestCase(0x02, 0x01, FlagState.Subtract)]
        [TestCase(0x7F, 0x7E, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x10, 0x0F, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x81, 0x80, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        public void DEC_xIndexOffset(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.IX = 0x5000;
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x80);
            WriteByteAtIndexAndOffset(WordRegister.IX, offset, input);

            ExecutionResult executionResult = ExecuteInstruction($"DEC (IX+o)", arg1: (byte)offset);

            Assert.That(ReadByteAtIndexAndOffset(WordRegister.IX, offset), Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        public void DEC_rr([Values(0x7F00, 0xFFFF, 0x0001, 0x0000)] int input, [Values(true, false)] bool carry)
        {
            Registers.BC = (ushort)input;
            Registers.Flags.Carry = carry;

            ExecutionResult executionResult = ExecuteInstruction($"DEC BC");
            ushort expectedResult = (ushort)(input - 1);

            Assert.That(Registers.BC, Is.EqualTo(expectedResult)); // no flags affected by DEC rr
        }
    }
}
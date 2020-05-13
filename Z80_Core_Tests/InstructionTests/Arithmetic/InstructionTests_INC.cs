using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_INC : InstructionTestBase
    {
        [Test]
        [TestCase(0x00, 0x01, FlagState.None)]
        [TestCase(0x0F, 0x10, FlagState.HalfCarry)]
        [TestCase(0xFF, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x80, 0x81, FlagState.Sign)]
        [TestCase(0x8F, 0x90, FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x7F, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void INC_r(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"INC A");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x00, 0x01, FlagState.None)]
        [TestCase(0x0F, 0x10, FlagState.HalfCarry)]
        [TestCase(0xFF, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x80, 0x81, FlagState.Sign)]
        [TestCase(0x8F, 0x90, FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x7F, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void INC_xHL(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, input);

            ExecutionResult executionResult = ExecuteInstruction($"INC (HL)");

            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedState));
        }


        [Test]
        [TestCase(0x00, 0x01, FlagState.None)]
        [TestCase(0x0F, 0x10, FlagState.HalfCarry)]
        [TestCase(0xFF, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x80, 0x81, FlagState.Sign)]
        [TestCase(0x8F, 0x90, FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x7F, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void INC_xIndexOffset(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.IX = 0x5000;
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x80);
            WriteByteAtIndexAndOffset(WordRegister.IX, offset, input);

            ExecutionResult executionResult = ExecuteInstruction($"INC (IX+o)", arg1: (byte)offset);

            Assert.That(ReadByteAtIndexAndOffset(WordRegister.IX, offset), Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        public void INC_rr([Values(0x7F00, 0xFFFF, 0x0001, 0x0000)] int input, [Values(true, false)] bool carry)
        {
            Registers.BC = (ushort)input;
            Registers.Flags.Carry = carry;

            ExecutionResult executionResult = ExecuteInstruction($"INC BC");
            ushort expectedResult = (ushort)(input + 1);

            Assert.That(Registers.BC, Is.EqualTo(expectedResult)); // no flags affected by INC rr
        }
    }
}
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_SUB : InstructionTestBase
    {
        [Test]
        [TestCase(0x01, 0x00, 0x01, FlagState.Subtract)]
        [TestCase(0x00, 0x90, 0x70, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x40, 0x40, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x10, 0xF0, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x80, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, 0x82, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void SUB_r(byte input, byte sub, byte expectedResult, FlagState expectedFlagState)
        {
            Registers.A = input;
            Registers.B = sub;

            ExecutionResult executionResult = ExecuteInstruction($"SUB B");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x01, 0x00, 0x01, FlagState.Subtract)]
        [TestCase(0x00, 0x90, 0x70, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x40, 0x40, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x10, 0xF0, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x80, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, 0x82, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void SUB_n(byte input, byte sub, byte expectedResult, FlagState expectedFlagState)
        {
            Registers.A = input;
            Registers.B = sub;

            ExecutionResult executionResult = ExecuteInstruction($"SUB n", arg1: sub);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x01, 0x00, 0x01, FlagState.Subtract)]
        [TestCase(0x00, 0x90, 0x70, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x40, 0x40, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x10, 0xF0, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x80, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, 0x82, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void SUB_xHL(byte input, byte sub, byte expectedResult, FlagState expectedFlagState)
        {
            Registers.A = input;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, sub);

            ExecutionResult executionResult = ExecuteInstruction($"SUB (HL)");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x01, 0x00, 0x01, FlagState.Subtract)]
        [TestCase(0x00, 0x90, 0x70, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x40, 0x40, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x10, 0xF0, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x80, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, 0x82, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void SUB_xIndexOffset(byte input, byte sub, byte expectedResult, FlagState expectedFlagState)
        {
            Registers.A = input;
            Registers.IX = 0x5000;
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x80);
            WriteByteAtIndexAndOffset(WordRegister.IX, offset, sub);

            ExecutionResult executionResult = ExecuteInstruction($"SUB (IX+o)", arg1: (byte)offset);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }
    }
}

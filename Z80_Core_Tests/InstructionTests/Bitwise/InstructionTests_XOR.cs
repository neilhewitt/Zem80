using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_XOR : InstructionTestBase
    {
        [Test]
        [TestCase(0x00, 0x01, 0x01, FlagState.None)]
        [TestCase(0x00, 0x03, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x80, 0x80, FlagState.Sign)]
        [TestCase(0x00, 0x81, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        public void XOR_r(byte first, byte second, byte expectedResult, FlagState expectedFlags)
        {
            Registers.A = first;
            Registers.B = second;

            ExecutionResult executionResult = ExecuteInstruction($"XOR B");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlags));
        }

        [Test]
        [TestCase(0x00, 0x01, 0x01, FlagState.None)]
        [TestCase(0x00, 0x03, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x80, 0x80, FlagState.Sign)]
        [TestCase(0x00, 0x81, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        public void XOR_n(byte first, byte second, byte expectedResult, FlagState expectedFlags)
        {
            Registers.A = first;

            ExecutionResult executionResult = ExecuteInstruction($"XOR n", arg1: second);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlags));
        }

        [Test]
        [TestCase(0x00, 0x01, 0x01, FlagState.None)]
        [TestCase(0x00, 0x03, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x80, 0x80, FlagState.Sign)]
        [TestCase(0x00, 0x81, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        public void XOR_xHL(byte first, byte second, byte expectedResult, FlagState expectedFlags)
        {
            Registers.A = first;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, second);

            ExecutionResult executionResult = ExecuteInstruction($"XOR (HL)");
            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlags));
        }

        [Test]
        [TestCase(0x00, 0x01, 0x01, FlagState.None)]
        [TestCase(0x00, 0x03, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x80, 0x80, FlagState.Sign)]
        [TestCase(0x00, 0x81, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        public void XOR_xIndexOffset(byte first, byte second, byte expectedResult, FlagState expectedFlags)
        {
            Registers.A = first;
            Registers.IX = 0x5000;
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x80);
            WriteByteAtIndexAndOffset(WordRegister.IX, offset, second);

            ExecutionResult executionResult = ExecuteInstruction($"XOR (IX+o)", arg1: (byte)offset);
            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlags));
        }
    }
}

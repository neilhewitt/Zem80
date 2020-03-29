using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_CP : InstructionTestBase
    {
        [Test]
        [TestCase(0x01, 0x00, FlagState.Subtract)]
        [TestCase(0x00, 0x90, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x40, 0x40, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x10, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void CP_r(byte first, byte second, FlagState state)
        {
            Registers.A = first;
            Registers.B = second;

            ExecutionResult executionResult = ExecuteInstruction($"CP B");

            Assert.That(executionResult.Flags.State, Is.EqualTo(state));
        }

        [Test]
        [TestCase(0x01, 0x00, FlagState.Subtract)]
        [TestCase(0x00, 0x90, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x40, 0x40, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x10, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void CP_n(byte first, byte second, FlagState state)
        {
            Registers.A = first;

            ExecutionResult executionResult = ExecuteInstruction($"CP n", arg1: second);

            Assert.That(executionResult.Flags.State, Is.EqualTo(state));
        }

        [Test]
        [TestCase(0x01, 0x00, FlagState.Subtract)]
        [TestCase(0x00, 0x90, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x40, 0x40, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x10, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void CP_xHL(byte first, byte second, FlagState state)
        {
            Registers.A = first;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, second);

            ExecutionResult executionResult = ExecuteInstruction($"CP (HL)");

            Assert.That(executionResult.Flags.State, Is.EqualTo(state));
        }

        [Test]
        [TestCase(0x01, 0x00, FlagState.Subtract)]
        [TestCase(0x00, 0x90, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x40, 0x40, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x10, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void CP_xIndexOffset(byte first, byte second, FlagState state)
        {
            Registers.A = first;
            Registers.IX = 0x5000;
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x80);
            WriteByteAtIndexAndOffset(WordRegister.IX, offset, second);

            ExecutionResult executionResult = ExecuteInstruction($"CP (IX+o)", arg1: (byte)offset);

            Assert.That(executionResult.Flags.State, Is.EqualTo(state));
        }
    }
}

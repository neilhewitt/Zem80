using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_ADD : InstructionTestBase
    {
        [Test]
        // FLAGS: ***V0*
        [TestCase(0x00, 0x01, 0x01, FlagState.None)]
        [TestCase(0x10, 0xF1, 0x01, FlagState.Carry)]
        [TestCase(0x01, 0x0F, 0x10, FlagState.HalfCarry)]
        [TestCase(0x02, 0xFF, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, 0x00, FlagState.Zero)]
        [TestCase(0x10, 0xF0, 0x00, FlagState.Carry | FlagState.Zero)]
        [TestCase(0x01, 0xFF, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x80, 0x80, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x7F, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void ADD_A_r(byte input, byte add, byte expectedResult, FlagState expectedFlagState)
        {
            Registers.A = input;
            Registers.B = add;

            ExecutionResult executionResult = ExecuteInstruction($"ADD A,B");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        // FLAGS: ***V0*
        [TestCase(0x00, 0x01, 0x01, FlagState.None)]
        [TestCase(0x10, 0xF1, 0x01, FlagState.Carry)]
        [TestCase(0x01, 0x0F, 0x10, FlagState.HalfCarry)]
        [TestCase(0x02, 0xFF, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, 0x00, FlagState.Zero)]
        [TestCase(0x10, 0xF0, 0x00, FlagState.Carry | FlagState.Zero)]
        [TestCase(0x01, 0xFF, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x80, 0x80, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x7F, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void ADD_A_n(byte input, byte add, byte expectedResult, FlagState expectedFlagState)
        {
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"ADD A,n", arg1: add);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x00, 0x01, 0x01, FlagState.None)]
        [TestCase(0x10, 0xF1, 0x01, FlagState.Carry)]
        [TestCase(0x01, 0x0F, 0x10, FlagState.HalfCarry)]
        [TestCase(0x02, 0xFF, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, 0x00, FlagState.Zero)]
        [TestCase(0x10, 0xF0, 0x00, FlagState.Carry | FlagState.Zero)]
        [TestCase(0x01, 0xFF, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x80, 0x80, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x7F, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void ADD_A_xHL(byte input, byte add, byte expectedResult, FlagState expectedFlagState)
        {
            Registers.A = input;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, add);

            ExecutionResult executionResult = ExecuteInstruction($"ADD A,n", arg1: add);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x00, 0x01, 0x01, FlagState.None)]
        [TestCase(0x10, 0xF1, 0x01, FlagState.Carry)]
        [TestCase(0x01, 0x0F, 0x10, FlagState.HalfCarry)]
        [TestCase(0x02, 0xFF, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, 0x00, FlagState.Zero)]
        [TestCase(0x10, 0xF0, 0x00, FlagState.Carry | FlagState.Zero)]
        [TestCase(0x01, 0xFF, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x80, 0x80, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x7F, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void ADD_A_xIndexOffset(byte input, byte add, byte expectedResult, FlagState expectedFlagState)
        {
            RegisterPairName indexRegister = RandomBool() ? RegisterPairName.IX : RegisterPairName.IY; // doesn't matter which as long as we exercise both
            sbyte offset = 0x7F;

            Registers.A = input;
            Registers[indexRegister] = 0x5000;
            WriteByteAtIndexAndOffset(indexRegister, offset, add);

            ExecutionResult executionResult = ExecuteInstruction($"ADD A,({ indexRegister }+o)", arg1: (byte)offset);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }
    }
}

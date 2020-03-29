using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_ADC : InstructionTestBase
    {
        [Test]
        // FLAGS: ***V0*
        [TestCase(0x00, 0x00, true, 0x01, FlagState.None)]
        [TestCase(0x00, 0x01, false, 0x01, FlagState.None)]
        [TestCase(0x10, 0xF0, true, 0x01, FlagState.Carry)]
        [TestCase(0x10, 0xF1, false, 0x01, FlagState.Carry)]
        [TestCase(0x01, 0x0F, true, 0x11, FlagState.HalfCarry)]
        [TestCase(0x01, 0x0F, false, 0x10, FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x02, 0xFF, false, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Zero)]
        [TestCase(0x10, 0xF0, false, 0x00, FlagState.Carry | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Carry | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x01, 0xFF, false, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x7F, true, 0x80, FlagState.Sign)]
        [TestCase(0x80, 0xFF, true, 0x80, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x00, 0x80, true, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x7F, true, 0x81, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, false, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void ADC_A_r(byte input, byte add, bool carry, byte expectedResult, FlagState expectedFlagState)
        {
            Flags.Carry = carry; 
            Registers.A = input;
            Registers.B = add;

            ExecutionResult executionResult = ExecuteInstruction($"ADC A,B");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        // FLAGS: ***V0*
        [TestCase(0x00, 0x00, true, 0x01, FlagState.None)]
        [TestCase(0x00, 0x01, false, 0x01, FlagState.None)]
        [TestCase(0x10, 0xF0, true, 0x01, FlagState.Carry)]
        [TestCase(0x10, 0xF1, false, 0x01, FlagState.Carry)]
        [TestCase(0x01, 0x0F, true, 0x11, FlagState.HalfCarry)]
        [TestCase(0x01, 0x0F, false, 0x10, FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x02, 0xFF, false, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Zero)]
        [TestCase(0x10, 0xF0, false, 0x00, FlagState.Carry | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Carry | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x01, 0xFF, false, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x7F, true, 0x80, FlagState.Sign)]
        [TestCase(0x80, 0xFF, true, 0x80, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x00, 0x80, true, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x7F, true, 0x81, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, false, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void ADC_A_n(byte input, byte add, bool carry, byte expectedResult, FlagState expectedFlagState)
        {
            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"ADC A,n", arg1: add);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x00, 0x00, true, 0x01, FlagState.None)]
        [TestCase(0x00, 0x01, false, 0x01, FlagState.None)]
        [TestCase(0x10, 0xF0, true, 0x01, FlagState.Carry)]
        [TestCase(0x10, 0xF1, false, 0x01, FlagState.Carry)]
        [TestCase(0x01, 0x0F, true, 0x11, FlagState.HalfCarry)]
        [TestCase(0x01, 0x0F, false, 0x10, FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x02, 0xFF, false, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Zero)]
        [TestCase(0x10, 0xF0, false, 0x00, FlagState.Carry | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Carry | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x01, 0xFF, false, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x7F, true, 0x80, FlagState.Sign)]
        [TestCase(0x80, 0xFF, true, 0x80, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x00, 0x80, true, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x7F, true, 0x81, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, false, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void ADC_A_xHL(byte input, byte add, bool carry, byte expectedResult, FlagState expectedFlagState)
        {
            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, add);

            ExecutionResult executionResult = ExecuteInstruction($"ADC A,n", arg1: add);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x00, 0x00, true, 0x01, FlagState.None)]
        [TestCase(0x00, 0x01, false, 0x01, FlagState.None)]
        [TestCase(0x10, 0xF0, true, 0x01, FlagState.Carry)]
        [TestCase(0x10, 0xF1, false, 0x01, FlagState.Carry)]
        [TestCase(0x01, 0x0F, true, 0x11, FlagState.HalfCarry)]
        [TestCase(0x01, 0x0F, false, 0x10, FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x02, 0xFF, false, 0x01, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Zero)]
        [TestCase(0x10, 0xF0, false, 0x00, FlagState.Carry | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Carry | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x01, 0xFF, false, 0x00, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x7F, true, 0x80, FlagState.Sign)]
        [TestCase(0x80, 0xFF, true, 0x80, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x00, 0x80, true, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x90, 0xF0, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x7F, true, 0x81, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, false, 0x80, FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x81, 0xFF, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void ADC_A_xIndexOffset(byte input, byte add, bool carry, byte expectedResult, FlagState expectedFlagState)
        {
            WordRegister indexRegister = RandomBool() ? WordRegister.IX : WordRegister.IY; // doesn't matter which as long as we exercise both
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x7F);

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers[indexRegister] = 0x5000;
            WriteByteAtIndexAndOffset(indexRegister, offset, add);

            ExecutionResult executionResult = ExecuteInstruction($"ADC A,({ indexRegister }+o)", arg1: (byte)offset);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x0000, 0x0000, true, 0x0001, FlagState.None)]
        [TestCase(0x1000, 0xF000, true, 0x0001, FlagState.Carry)]
        [TestCase(0x0100, 0x0F00, true, 0x1001, FlagState.HalfCarry)]
        [TestCase(0x0100, 0xFF00, true, 0x0001, FlagState.Carry | FlagState.HalfCarry)]
        [TestCase(0x0000, 0x0000, false, 0x0000, FlagState.Zero)]
        [TestCase(0x0000, 0xFFFF, true, 0x0000, FlagState.Carry | FlagState.Zero)]
        [TestCase(0x0100, 0x0F00, false, 0x1000, FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x0100, 0xFF00, false, 0x0000, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x0000, 0x8000, true, 0x8001, FlagState.Sign)]
        [TestCase(0x9000, 0xF000, true, 0x8001, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x0100, 0x7F00, true, 0x8001, FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x8100, 0xFF00, true, 0x8001, FlagState.Carry | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x0000, 0x7FFF, true, 0x8000, FlagState.Zero | FlagState.Sign)]
        [TestCase(0x8000, 0xFFFF, true, 0x8000, FlagState.Carry | FlagState.Zero | FlagState.Sign)]
        [TestCase(0x0100, 0x7F00, false, 0x8000, FlagState.HalfCarry | FlagState.Zero | FlagState.Sign)]
        [TestCase(0x8100, 0xFF00, false, 0x8000, FlagState.Carry | FlagState.HalfCarry | FlagState.Zero | FlagState.Sign)]
        public void ADC_HL_rr(int input, int add, bool carry, int expectedResult, FlagState expectedFlagState)
        {
            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.HL = (ushort)input;
            Registers.DE = (ushort)add;

            ExecutionResult executionResult = ExecuteInstruction($"ADC HL,DE");

            Assert.That(Registers.HL, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }
    }
}

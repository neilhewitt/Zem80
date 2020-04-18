using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_SBC : InstructionTestBase
    {
        private (byte, Flags) GetExpectedResultAndFlags(byte value, byte subtract, bool carry)
        {
            Flags flags = new Flags();

            int result = value - subtract - (carry ? 1 : 0);
            flags = FlagLookup.ByteArithmeticFlags(value, subtract, carry, true);
            flags.Subtract = true;

            return ((byte)result, flags);
        }

        private (ushort, Flags) GetExpectedResultAndFlags(ushort value, ushort subtract, bool carry)
        {
            Flags flags = new Flags();

            int result = value - subtract - (carry ? 1 : 0);
            flags = FlagLookup.WordArithmeticFlags(flags, value, subtract, carry, true, true);
            flags.Subtract = true;

            return ((ushort)result, flags);
        }

        [Test]
        [TestCase(0x02, 0x00, true, 0x01, FlagState.Subtract)]
        [TestCase(0x01, 0x00, false, 0x01, FlagState.Subtract)]
        [TestCase(0x00, 0x80, true, 0x7F, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x90, false, 0x70, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, false, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, true, 0x7E, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, false, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, true, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x41, 0x40, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x40, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x10, 0x0F, true, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x00, true, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x10, false, 0xF0, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x01, 0x80, true, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, true, 0xFE, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x00, 0x01, false, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, true, 0x81, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, false, 0x82, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void SBC_A_r(byte input, byte sub, bool carry, byte expectedResult, FlagState expectedFlagState)
        {
            Flags.Carry = carry;
            Registers.A = input;
            Registers.B = sub;

            ExecutionResult executionResult = ExecuteInstruction($"SBC A,B");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x02, 0x00, true, 0x01, FlagState.Subtract)]
        [TestCase(0x01, 0x00, false, 0x01, FlagState.Subtract)]
        [TestCase(0x00, 0x80, true, 0x7F, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x90, false, 0x70, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, false, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, true, 0x7E, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, false, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, true, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x41, 0x40, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x40, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x10, 0x0F, true, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x00, true, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x10, false, 0xF0, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x01, 0x80, true, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, true, 0xFE, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x00, 0x01, false, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, true, 0x81, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, false, 0x82, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void SBC_A_n(byte input, byte sub, bool carry, byte expectedResult, FlagState expectedFlagState)
        {
            Flags.Carry = carry;
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"SBC A,n", arg1: sub);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x02, 0x00, true, 0x01, FlagState.Subtract)]
        [TestCase(0x01, 0x00, false, 0x01, FlagState.Subtract)]
        [TestCase(0x00, 0x80, true, 0x7F, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x90, false, 0x70, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, false, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, true, 0x7E, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, false, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, true, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x41, 0x40, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x40, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x10, 0x0F, true, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x00, true, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x10, false, 0xF0, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x01, 0x80, true, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, true, 0xFE, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x00, 0x01, false, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, true, 0x81, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, false, 0x82, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void SBC_A_xHL(byte input, byte sub, bool carry, byte expectedResult, FlagState expectedFlagState)
        {
            Flags.Carry = carry;
            Registers.A = input;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, sub);

            ExecutionResult executionResult = ExecuteInstruction($"SBC A,(HL)", arg1: sub);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x02, 0x00, true, 0x01, FlagState.Subtract)]
        [TestCase(0x01, 0x00, false, 0x01, FlagState.Subtract)]
        [TestCase(0x00, 0x80, true, 0x7F, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x90, false, 0x70, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, false, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, true, 0x7E, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x00, 0x81, false, 0x7F, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, true, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x41, 0x40, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x40, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x10, 0x0F, true, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x00, true, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x10, false, 0xF0, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x01, 0x80, true, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x01, true, 0xFE, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x00, 0x01, false, 0xFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, true, 0x81, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, false, 0x82, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void SBC_A_xIndexOffset(byte input, byte sub, bool carry, byte expectedResult, FlagState expectedFlagState)            
        {
            WordRegister indexRegister = RandomBool() ? WordRegister.IX : WordRegister.IY; // doesn't matter which as long as we exercise both
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x7F);

            Flags.Carry = carry;
            Registers.A = input;
            Registers[indexRegister] = 0x5000;
            WriteByteAtIndexAndOffset(indexRegister, offset, sub);

            ExecutionResult executionResult = ExecuteInstruction($"SBC A,({indexRegister}+o)", arg1: (byte)offset);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }

        [Test]
        [TestCase(0x0100, 0x0000, true, 0x00FF, FlagState.Subtract)]
        [TestCase(0x0000, 0x8000, true, 0x7FFF, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x0100, 0xFF00, true, 0x01FF, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x0000, 0x8100, true, 0x7EFF, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x0000, 0x0000, false, 0x0000, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x0000, 0x80FF, true, 0x7F00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x0100, 0xFF00, false, 0x0200, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x0000, 0x8100, false, 0x7F00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x0000, 0x0000, true, 0xFFFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x0100, 0x8000, true, 0x80FF, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x0000, 0x0100, true, 0xFEFF, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x0100, 0x7F00, true, 0x81FF, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x0000, 0x00FF, true, 0xFF00, FlagState.Subtract | FlagState.Zero | FlagState.Sign)]
        [TestCase(0x0000, 0x8000, false, 0x8000, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero | FlagState.Sign)]
        [TestCase(0x0000, 0x0100, false, 0xFF00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero | FlagState.Sign)]
        [TestCase(0x0100, 0x7F00, false, 0x8200, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero | FlagState.Sign)]
        public void SBC_HL_rr(int input, int sub, bool carry, int expectedResult, FlagState expectedFlagState)
        {
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x7F);

            Flags.Carry = carry;
            Registers.HL = (ushort)input;
            Registers.DE = (ushort)sub;

            ExecutionResult executionResult = ExecuteInstruction($"SBC HL,DE", arg1: (byte)offset);

            Assert.That(Registers.HL, Is.EqualTo((ushort)expectedResult));
            Assert.That(executionResult.Flags.State, Is.EqualTo(expectedFlagState));
        }
    }
}

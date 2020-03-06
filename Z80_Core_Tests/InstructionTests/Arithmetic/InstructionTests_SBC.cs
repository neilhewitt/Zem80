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
            flags = FlagLookup.FlagsFromArithmeticOperation(value, subtract, carry, true);
            flags.Subtract = true;

            return ((byte)result, flags);
        }

        private (ushort, Flags) GetExpectedResultAndFlags(ushort value, ushort subtract, bool carry)
        {
            Flags flags = new Flags();

            int result = value - subtract - (carry ? 1 : 0);
            flags = FlagLookup.FlagsFromArithmeticOperation16Bit(flags, value, subtract, carry, true, true);
            flags.Subtract = true;

            return ((ushort)result, flags);
        }

        [Test]
        [TestCase(0x02, 0x00, true, 0x01, FlagState.Subtract)]
        [TestCase(0x01, 0x00, false, 0x01, FlagState.Subtract)]
        [TestCase(0x00, 0x80, true, 0x7F, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x81, false, 0x7F, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, false, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0x8F, true, 0x71, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x8F, false, 0x72, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, true, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x40, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x09, 0x08, true, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x08, 0x08, false, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x49, 0x48, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x48, 0x48, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x00, true, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x01, false, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x01, 0x80, true, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x0F, true, 0xF1, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x0F, false, 0xF2, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
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
        [TestCase(0x00, 0x81, false, 0x7F, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, false, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0x8F, true, 0x71, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x8F, false, 0x72, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, true, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x40, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x09, 0x08, true, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x08, 0x08, false, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x49, 0x48, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x48, 0x48, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x00, true, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x01, false, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x01, 0x80, true, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x0F, true, 0xF1, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x0F, false, 0xF2, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
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
        [TestCase(0x00, 0x81, false, 0x7F, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, false, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0x8F, true, 0x71, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x8F, false, 0x72, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, true, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x40, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x09, 0x08, true, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x08, 0x08, false, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x49, 0x48, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x48, 0x48, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x00, true, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x01, false, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x01, 0x80, true, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x0F, true, 0xF1, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x0F, false, 0xF2, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
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
        [TestCase(0x00, 0x81, false, 0x7F, FlagState.Subtract | FlagState.ParityOverflow)]
        [TestCase(0x01, 0xFF, true, 0x01, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0xFF, false, 0x02, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x01, 0x8F, true, 0x71, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x8F, false, 0x72, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry)]
        [TestCase(0x01, 0x00, true, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0x00, false, 0x00, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00, 0xFF, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x40, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x09, 0x08, true, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x08, 0x08, false, 0x00, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x49, 0x48, true, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x48, 0x48, false, 0x00, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x00, 0x00, true, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00, 0x01, false, 0xFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x01, 0x80, true, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x00, 0x80, false, 0x80, FlagState.Subtract | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x01, 0x0F, true, 0xF1, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x0F, false, 0xF2, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, true, 0x81, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x01, 0x7F, false, 0x82, FlagState.Subtract | FlagState.ParityOverflow | FlagState.HalfCarry | FlagState.Sign)]
        public void SBC_A_xIndexOffset(byte input, byte sub, bool carry, byte expectedResult, FlagState expectedFlagState)            
        {
            RegisterPairName indexRegister = RandomBool() ? RegisterPairName.IX : RegisterPairName.IY; // doesn't matter which as long as we exercise both
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
        [TestCase(0x0000, 0x8000, true, 0x7FFF, FlagState.Subtract)]
        [TestCase(0x00FF, 0x8F01, true, 0x71FD, FlagState.Subtract | FlagState.HalfCarry)]
        [TestCase(0x0000, 0x0000, false, 0x0000, FlagState.Subtract | FlagState.Zero)]
        [TestCase(0x00FF, 0x8FFE, true, 0x7100, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero)]
        [TestCase(0x0000, 0x0000, true, 0xFFFF, FlagState.Subtract | FlagState.Sign)]
        [TestCase(0x00FF, 0x0F01, true, 0xF1FD, FlagState.Subtract | FlagState.HalfCarry | FlagState.Sign)]
        [TestCase(0x0000, 0x00FF, true, 0xFF00, FlagState.Subtract | FlagState.Zero | FlagState.Sign)]
        [TestCase(0x00FF, 0x0FFE, true, 0xF100, FlagState.Subtract | FlagState.HalfCarry | FlagState.Zero | FlagState.Sign)]
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

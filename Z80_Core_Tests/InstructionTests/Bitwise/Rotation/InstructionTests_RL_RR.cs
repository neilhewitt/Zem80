using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;
using System.Linq;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RL_RR : InstructionTestBase
    {
        [Test]
        [TestCase(0x01, true, 0x03, FlagState.None)]
        [TestCase(0x01, false, 0x02, FlagState.None)]
        [TestCase(0x80, true, 0x01, FlagState.Carry)]
        [TestCase(0x80, false, 0x00, FlagState.Carry)]
        [TestCase(0x03, true, 0x07, FlagState.ParityOverflow)]
        [TestCase(0x03, false, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x81, true, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x81, false, 0x02, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, true, 0x01, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, false, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, true, 0x81, FlagState.Sign)]
        [TestCase(0x40, false, 0x80, FlagState.Sign)]
        [TestCase(0xC1, true, 0x83, FlagState.Carry | FlagState.Sign)]
        [TestCase(0xC1, false, 0x82, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x41, true, 0x83, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x41, false, 0x82, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        public void RL_A(byte input, bool carry, byte expectedResult, FlagState expectedState)
        {
            Flags.Carry = carry;
            Registers.A = input; // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RL A");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x02, true, 0x81, FlagState.None)]
        [TestCase(0x02, false, 0x01, FlagState.None)]
        [TestCase(0x80, true, 0xC0, FlagState.Carry)]
        [TestCase(0x80, false, 0x40, FlagState.Carry)]
        [TestCase(0x06, true, 0x83, FlagState.ParityOverflow)]
        [TestCase(0x06, false, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x82, true, 0xC1, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x82, false, 0x41, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, true, 0x80, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, false, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x01, true, 0x80, FlagState.Sign)]
        [TestCase(0x01, false, 0x00, FlagState.Sign)]
        [TestCase(0x83, true, 0xC1, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x83, false, 0x41, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x03, true, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x03, false, 0x01, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, true, 0xC0, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, false, 0x40, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        public void RR_B(byte input, bool carry, byte expectedResult, FlagState expectedState)
        {
            Flags.Carry = carry;
            Registers.B = input; // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RR B");

            Assert.That(Registers.B, Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x01, true, 0x03, FlagState.None)]
        [TestCase(0x01, false, 0x02, FlagState.None)]
        [TestCase(0x80, true, 0x01, FlagState.Carry)]
        [TestCase(0x80, false, 0x00, FlagState.Carry)]
        [TestCase(0x03, true, 0x07, FlagState.ParityOverflow)]
        [TestCase(0x03, false, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x81, true, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x81, false, 0x02, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, true, 0x01, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, false, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, true, 0x81, FlagState.Sign)]
        [TestCase(0x40, false, 0x80, FlagState.Sign)]
        [TestCase(0xC1, true, 0x83, FlagState.Carry | FlagState.Sign)]
        [TestCase(0xC1, false, 0x82, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x41, true, 0x83, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x41, false, 0x82, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        public void RL_xHL(byte input, bool carry, byte expectedResult, FlagState expectedState)
        {
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, input);
            Flags.Carry = carry;

            ExecutionResult executionResult = ExecuteInstruction($"RL (HL)");

            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x02, true, 0x81, FlagState.None)]
        [TestCase(0x02, false, 0x01, FlagState.None)]
        [TestCase(0x80, true, 0xC0, FlagState.Carry)]
        [TestCase(0x80, false, 0x40, FlagState.Carry)]
        [TestCase(0x06, true, 0x83, FlagState.ParityOverflow)]
        [TestCase(0x06, false, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x82, true, 0xC1, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x82, false, 0x41, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, true, 0x80, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, false, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x01, true, 0x80, FlagState.Sign)]
        [TestCase(0x01, false, 0x00, FlagState.Sign)]
        [TestCase(0x83, true, 0xC1, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x83, false, 0x41, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x03, true, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x03, false, 0x01, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, true, 0xC0, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, false, 0x40, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        public void RR_xHL(byte input, bool carry, byte expectedResult, FlagState expectedState)
        {
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, input);
            Flags.Carry = carry;

            ExecutionResult executionResult = ExecuteInstruction($"RR (HL)");

            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x01, true, 0x03, FlagState.None)]
        [TestCase(0x01, false, 0x02, FlagState.None)]
        [TestCase(0x80, true, 0x01, FlagState.Carry)]
        [TestCase(0x80, false, 0x00, FlagState.Carry)]
        [TestCase(0x03, true, 0x07, FlagState.ParityOverflow)]
        [TestCase(0x03, false, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x81, true, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x81, false, 0x02, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, true, 0x01, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, false, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, true, 0x81, FlagState.Sign)]
        [TestCase(0x40, false, 0x80, FlagState.Sign)]
        [TestCase(0xC1, true, 0x83, FlagState.Carry | FlagState.Sign)]
        [TestCase(0xC1, false, 0x82, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x41, true, 0x83, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x41, false, 0x82, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, true, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, false, 0x80, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        public void RL_xIndexOffset(byte input, bool carry, byte expectedResult, FlagState expectedState)
        {
            sbyte offset = 0x7F;
            Registers.IX = 0x5000;
            WriteByteAtIndexAndOffset(RegisterWord.IX, offset, input);
            Flags.Carry = carry;

            ExecutionResult executionResult = ExecuteInstruction($"RL (IX+o)", arg1: (byte)offset);

            Assert.That(ReadByteAtIndexAndOffset(RegisterWord.IX, offset), Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x02, true, 0x81, FlagState.None)]
        [TestCase(0x02, false, 0x01, FlagState.None)]
        [TestCase(0x80, true, 0xC0, FlagState.Carry)]
        [TestCase(0x80, false, 0x40, FlagState.Carry)]
        [TestCase(0x06, true, 0x83, FlagState.ParityOverflow)]
        [TestCase(0x06, false, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x82, true, 0xC1, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x82, false, 0x41, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, true, 0x80, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, false, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x01, true, 0x80, FlagState.Sign)]
        [TestCase(0x01, false, 0x00, FlagState.Sign)]
        [TestCase(0x83, true, 0xC1, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x83, false, 0x41, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x03, true, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x03, false, 0x01, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, true, 0xC0, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, false, 0x40, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        public void RR_xIndexOffset(byte input, bool carry, byte expectedResult, FlagState expectedState)
        {
            sbyte offset = -0x80;
            Registers.IY = 0x5000;
            WriteByteAtIndexAndOffset(RegisterWord.IY, offset, input);
            Flags.Carry = carry;

            ExecutionResult executionResult = ExecuteInstruction($"RR (IY+o)", arg1: (byte)offset);

            Assert.That(ReadByteAtIndexAndOffset(RegisterWord.IY, offset), Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }
    }
}
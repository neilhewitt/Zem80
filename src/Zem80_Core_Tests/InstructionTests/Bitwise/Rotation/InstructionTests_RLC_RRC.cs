using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RLC_RRC : InstructionTestBase
    {
        [Test]
        [TestCase(0x01, 0x02, FlagState.None)]
        [TestCase(0x01, 0x02, FlagState.None)]
        [TestCase(0x80, 0x01, FlagState.Carry)]
        [TestCase(0x80, 0x01, FlagState.Carry)]
        [TestCase(0x03, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x03, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x81, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x81, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x80, FlagState.Sign)]
        [TestCase(0x40, 0x80, FlagState.Sign)]
        [TestCase(0xC1, 0x83, FlagState.Carry | FlagState.Sign)]
        [TestCase(0xC1, 0x83, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x41, 0x82, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x41, 0x82, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        public void RLC_A(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.A = input; // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RLC A");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x02, 0x01, FlagState.None)]
        [TestCase(0x02, 0x01, FlagState.None)]
        [TestCase(0x80, 0x40, FlagState.Carry)]
        [TestCase(0x80, 0x40, FlagState.Carry)]
        [TestCase(0x06, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x06, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x82, 0x41, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x82, 0x41, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x01, 0x80, FlagState.Sign)]
        [TestCase(0x01, 0x80, FlagState.Sign)]
        [TestCase(0x83, 0xC1, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x83, 0xC1, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x03, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x03, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, 0xC0, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, 0xC0, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        public void RRC_B(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.B = input; // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RRC B");

            Assert.That(Registers.B, Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }


        [Test]
        [TestCase(0x01, 0x02, FlagState.None)]
        [TestCase(0x01, 0x02, FlagState.None)]
        [TestCase(0x80, 0x01, FlagState.Carry)]
        [TestCase(0x80, 0x01, FlagState.Carry)]
        [TestCase(0x03, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x03, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x81, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x81, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x80, FlagState.Sign)]
        [TestCase(0x40, 0x80, FlagState.Sign)]
        [TestCase(0xC1, 0x83, FlagState.Carry | FlagState.Sign)]
        [TestCase(0xC1, 0x83, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x41, 0x82, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x41, 0x82, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        public void RLC_xHL(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, input); // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RLC (HL)");

            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x02, 0x01, FlagState.None)]
        [TestCase(0x02, 0x01, FlagState.None)]
        [TestCase(0x80, 0x40, FlagState.Carry)]
        [TestCase(0x80, 0x40, FlagState.Carry)]
        [TestCase(0x06, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x06, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x82, 0x41, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x82, 0x41, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x01, 0x80, FlagState.Sign)]
        [TestCase(0x01, 0x80, FlagState.Sign)]
        [TestCase(0x83, 0xC1, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x83, 0xC1, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x03, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x03, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, 0xC0, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, 0xC0, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        public void RRC_xHL(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, input); // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RRC (HL)");

            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x01, 0x02, FlagState.None)]
        [TestCase(0x01, 0x02, FlagState.None)]
        [TestCase(0x80, 0x01, FlagState.Carry)]
        [TestCase(0x80, 0x01, FlagState.Carry)]
        [TestCase(0x03, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x03, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x81, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x81, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x40, 0x80, FlagState.Sign)]
        [TestCase(0x40, 0x80, FlagState.Sign)]
        [TestCase(0xC1, 0x83, FlagState.Carry | FlagState.Sign)]
        [TestCase(0xC1, 0x83, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x41, 0x82, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x41, 0x82, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0xC0, 0x81, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]

        public void RLC_xIndexOffset(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.IX = 0x5000;
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x80);
            WriteByteAtIndexAndOffset(WordRegister.IX, offset, input); // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RLC (IX+o)", arg1: (byte)offset);

            Assert.That(ReadByteAtIndexAndOffset(WordRegister.IX, offset), Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x02, 0x01, FlagState.None)]
        [TestCase(0x02, 0x01, FlagState.None)]
        [TestCase(0x80, 0x40, FlagState.Carry)]
        [TestCase(0x80, 0x40, FlagState.Carry)]
        [TestCase(0x06, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x06, 0x03, FlagState.ParityOverflow)]
        [TestCase(0x82, 0x41, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x82, 0x41, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        [TestCase(0x01, 0x80, FlagState.Sign)]
        [TestCase(0x01, 0x80, FlagState.Sign)]
        [TestCase(0x83, 0xC1, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x83, 0xC1, FlagState.Carry | FlagState.Sign)]
        [TestCase(0x03, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x03, 0x81, FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, 0xC0, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]
        [TestCase(0x81, 0xC0, FlagState.Carry | FlagState.ParityOverflow | FlagState.Sign)]

        public void RRC_xIndexOffset(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.IY = 0x5000;
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x80);
            WriteByteAtIndexAndOffset(WordRegister.IY, offset, input); // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RRC (IY+o)", arg1: (byte)offset);

            Assert.That(ReadByteAtIndexAndOffset(WordRegister.IY, offset), Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }
    }
}
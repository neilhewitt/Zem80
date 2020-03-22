using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_SRL : InstructionTestBase
    {
        [Test]
        [TestCase(0x04, 0x02, FlagState.None)]
        [TestCase(0x02, 0x01, FlagState.Carry)]
        [TestCase(0x0C, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x06, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        public void SRL_A(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.A = input; // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"SRL A");

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x04, 0x02, FlagState.None)]
        [TestCase(0x02, 0x01, FlagState.Carry)]
        [TestCase(0x0C, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x06, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        public void SRL_xHL(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, input); // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"SRL (HL)");

            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }

        [Test]
        [TestCase(0x04, 0x02, FlagState.None)]
        [TestCase(0x02, 0x01, FlagState.Carry)]
        [TestCase(0x0C, 0x06, FlagState.ParityOverflow)]
        [TestCase(0x06, 0x03, FlagState.Carry | FlagState.ParityOverflow)]
        [TestCase(0x00, 0x00, FlagState.ParityOverflow | FlagState.Zero)]
        public void SRL_xIndexOffset(byte input, byte expectedResult, FlagState expectedState)
        {
            Registers.IX = 0x5000;
            sbyte offset = (sbyte)(RandomBool() ? 0x7F : -0x80);
            WriteByteAtIndexAndOffset(RegisterWord.IX, offset, input); // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"SRL (IX+o)", arg1: (byte)offset);

            Assert.That(ReadByteAtIndexAndOffset(RegisterWord.IX, offset), Is.EqualTo(expectedResult));
            Assert.That(CPU.Registers.Flags.State, Is.EqualTo(expectedState));
        }
    }
}
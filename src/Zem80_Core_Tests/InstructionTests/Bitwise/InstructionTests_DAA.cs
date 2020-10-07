using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_DAA : InstructionTestBase
    {
        [Test]
        public void DAA_HalfCarry()
        {
            byte input = 0x6B; // 0110 (6) 1011 (BCD overflow)
            byte expected = 0x71;

            Registers.A = input;
            Flags.HalfCarry = true;
            ExecutionResult executionResult = ExecuteInstruction("DAA");

            Assert.That(Registers.A, Is.EqualTo(expected));
            Assert.That(
                executionResult.Flags.Check(
                    halfCarry: true,
                    zero: false,
                    sign: false,
                    parityOverflow: false
                    ),
                Is.True);
        }

        [Test]
        public void DAA_Carry()
        {
            byte input = 0xB6; // 1011 (BCD overflow) 0110 (6)
            byte expected = 0x16;

            Registers.A = input;
            Flags.Carry = true;
            ExecutionResult executionResult = ExecuteInstruction("DAA");

            Assert.That(Registers.A, Is.EqualTo(expected));
            Assert.That(
                executionResult.Flags.Check(
                    carry: true,
                    zero: false,
                    sign: false,
                    parityOverflow: false
                    ),
                Is.True);
        }

        [Test]
        public void DAA_LowOverflow()
        {
            byte input = 0x3F; // 1011 (BCD overflow) 0110 (6)
            byte expected = 0x45;

            Registers.A = input;
            ExecutionResult executionResult = ExecuteInstruction("DAA");

            Assert.That(Registers.A, Is.EqualTo(expected));
            Assert.That(
                executionResult.Flags.Check(
                    carry: false,
                    zero: false,
                    sign: false,
                    parityOverflow: false
                    ),
                Is.True);
        }

        [Test]
        public void DAA_HighOverflow()
        {
            byte input = 0xF3; // 1011 (BCD overflow) 0110 (6)
            byte expected = 0x53;

            Registers.A = input;
            ExecutionResult executionResult = ExecuteInstruction("DAA");

            Assert.That(Registers.A, Is.EqualTo(expected));
            Assert.That(
                executionResult.Flags.Check(
                    carry: true,
                    zero: false,
                    sign: false,
                    parityOverflow: true
                    ),
                Is.True);
        }
    }
}
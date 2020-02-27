using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RLCA_RRCA : InstructionTestBase
    {
        [Test]
        public void RLCA()
        {
            byte value = 0x7F;
            byte expected = ((byte)(value << 1)).SetBit(0, value.GetBit(7));
            bool carry = value.GetBit(7);
            Registers.A = value;
            Registers.Flags.Carry = carry;

            ExecutionResult executionResult = ExecuteInstruction("RLCA");

            Assert.That(Registers.A, Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags.Carry, Is.EqualTo(carry));
        }

        [Test]
        public void RRCA()
        {
            byte value = 0x7F;
            byte expected = ((byte)(value >> 1)).SetBit(7, value.GetBit(0));
            bool carry = value.GetBit(0);
            Registers.A = value;
            Registers.Flags.Carry = carry;

            ExecutionResult executionResult = ExecuteInstruction("RRCA");

            Assert.That(Registers.A, Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags.Carry, Is.EqualTo(carry));
        }
    }
}

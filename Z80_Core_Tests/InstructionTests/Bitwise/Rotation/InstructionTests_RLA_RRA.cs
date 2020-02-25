using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RLA_RRA : InstructionTestBase
    {
        [Test]
        public void RLA([Values(true, false)] bool previousCarry)
        {
            byte value = RandomByte();
            byte expected = ((byte)(value << 1)).SetBit(0, previousCarry);
            bool carry = value.GetBit(7);
            Registers.A = value;
            Registers.Flags.Carry = previousCarry;

            ExecutionResult executionResult = ExecuteInstruction("RLA");

            Assert.That(Registers.A == expected && Registers.Flags.Carry == carry);
        }

        [Test]
        public void RRA([Values(true, false)] bool previousCarry)
        {
            byte value = RandomByte();
            byte expected = ((byte)(value >> 1)).SetBit(7, previousCarry);
            bool carry = value.GetBit(0);
            Registers.A = value;
            Registers.Flags.Carry = previousCarry;

            ExecutionResult executionResult = ExecuteInstruction("RRA");

            Assert.That(Registers.A == expected && Registers.Flags.Carry == carry);
        }
    }
}

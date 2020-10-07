using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    public class InstructionTests_CCF : InstructionTestBase
    {
        [Test]
        public void CCF()
        {
            Registers.Flags.HalfCarry = true;
            Registers.Flags.Carry = false;

            ExecutionResult executionResult = ExecuteInstruction("CCF"); // inverts carry, preserves half-carry, resets subtract

            Assert.That(Registers.Flags.HalfCarry, Is.True);
            Assert.That(Registers.Flags.Carry, Is.True);
            Assert.That(Registers.Flags.Subtract, Is.False);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_CPL : InstructionTestBase
    {
        [Test]
        public void CPL()
        {
            byte value = 0x7F;
            byte expected = value ^= value; // CPL XORs the value in A

            Registers.A = value;
            ExecutionResult executionResult = ExecuteInstruction("CPL");

            Assert.That(Registers.A, Is.EqualTo(expected));
            Assert.That(
                executionResult.Flags.Check(
                    halfCarry: true,
                    subtract: true
                    ),
                Is.True);
        }
    }
}

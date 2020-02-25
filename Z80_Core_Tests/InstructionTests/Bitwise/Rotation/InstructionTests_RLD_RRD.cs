using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RLD_RRD : InstructionTestBase
    {
        [Test]
        public void RLD()
        {
            // Expected:
            // 0x7A -> 0x73 - A
            // 0x31 -> 0x1A - (HL)
            Registers.A = 0x7A;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, 0x31);

            ExecutionResult executionResult = ExecuteInstruction("RLD");

            Assert.That(Registers.A == 0x73 && ReadByteAt(Registers.HL) == 0x1A);
        }

        [Test]
        public void RRD()
        {
            // Expected:
            // 0x84 -> 0x80 - A
            // 0x20 -> 0x42 - (HL)
            Registers.A = 0x84;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, 0x20);

            ExecutionResult executionResult = ExecuteInstruction("RRD");

            Assert.That(Registers.A == 0x80 && ReadByteAt(Registers.HL) == 0x42);
        }
    }
}

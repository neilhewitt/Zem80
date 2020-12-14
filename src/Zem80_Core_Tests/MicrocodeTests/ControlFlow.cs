using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using Zem80.Core.Instructions;

namespace Zem80.Core.Tests.MicrocodeTests
{
    [TestFixture]
    public class ControlFlow : MicrocodeTestBase
    {
        [Test]
        // JP nn - jump to absolute address nn
        public void JPnn()
        {
            ExecuteInstruction("JP nn", 0x24, 0x05);
            Assert.That(Registers.PC == 0x0524);
        }

        [TestCase(Condition.Z)]
        [TestCase(Condition.NZ)]
        [TestCase(Condition.C)]
        [TestCase(Condition.NC)]
        [TestCase(Condition.M)]
        [TestCase(Condition.P)]
        [TestCase(Condition.PE)]
        [TestCase(Condition.PO)]
        // JP condition, nn - jump to absolute address if condition is satisfied by flags
        // we test both the not case (condition is not satisfied, program counter set to start of next instruction)
        // and the true case (condition is satisfied, program counter set to absolute address nn) for each condition
        public void JPcondition(Condition condition)
        {
            Registers.Flags.SetFromCondition(condition); // set flag condition according to test case value
            Registers.Flags.Value = Registers.Flags.Value.Invert(); // explicitly reverse the flag condition (for initial fail)

            Registers.PC = 0;
            ExecuteInstruction("JP " + condition.ToString() + ",nn", 0x10, 0x30);
            Assert.That(Registers.PC == 3); // condition was not true so PC = instruction address (0) + instruction length in bytes (3) so PC should == 3

            Registers.Flags.SetFromCondition(condition);
            ExecuteInstruction("JP " + condition.ToString() + ",nn", 0x10, 0x30); // condition was true so PC should == 0x3010
            Assert.That(Registers.PC == 0x3010);
        }

        [TestCase(0x4000, 0x10, 0x4012)] // +16 bytes, +2 bytes for instruction = jump 18 bytes to 0x4012
        [TestCase(0x4000, 0xFF, 0x3F82)] // -128 bytes, +2 bytes for instruction = jump -126 bytes to 0x3F82
        // JR o - jump forward / back relative to this instruction address
        // o is a signed byte (-128 to +127) but the jump address is adjusted by +2 bytes to accomodate for the JR instruction length itself
        // so the actual jump range is -126 to +129, and we need to test this behaviour
        public void JRo(int address, int displacement, int expectedAddress)
        {
            Registers.PC = (ushort)address;
            ExecuteInstruction("JR o", (byte)displacement);
            Assert.That(Registers.PC == (ushort)expectedAddress);
        }
    }
}

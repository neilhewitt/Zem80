using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_EI_DI : InstructionTestBase
    {
        [Test]
        public void EI()
        {
            bool interruptsBefore = CPU.InterruptsEnabled;
            ExecutionResult executionResult = ExecuteInstruction("EI");
            bool interruptsAfter = CPU.InterruptsEnabled;

            Assert.That(interruptsBefore, Is.False);
            Assert.That(interruptsAfter, Is.True);
        }

        [Test]
        public void DI()
        {
            bool interruptsBefore = CPU.InterruptsEnabled;
            ExecutionResult executionResult = ExecuteInstruction("DI");
            bool interruptsAfter = CPU.InterruptsEnabled;

            Assert.That(interruptsBefore, Is.True);
            Assert.That(interruptsAfter, Is.False);
        }
    }
}
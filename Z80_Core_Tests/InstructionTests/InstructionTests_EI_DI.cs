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
            bool interruptsBefore = _cpu.InterruptsEnabled;
            Execute("EI");
            bool interruptsAfter = _cpu.InterruptsEnabled;

            Assert.That(interruptsBefore == false && interruptsAfter == true);
        }

        [Test]
        public void DI()
        {
            bool interruptsBefore = _cpu.InterruptsEnabled;
            Execute("DI");
            bool interruptsAfter = _cpu.InterruptsEnabled;

            Assert.That(interruptsBefore == true && interruptsAfter == false);
        }
    }
}
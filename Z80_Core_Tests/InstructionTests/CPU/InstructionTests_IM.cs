using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_IM : InstructionTestBase
    {
        [Test]
        public void IM([Range(0, 2)] int mode)
        {
            ExecuteInstruction($"IM {mode}");
            Assert.That(CPU.InterruptMode, Is.EqualTo((InterruptMode)mode));
            Assert.That(CPU.InterruptsEnabled, Is.True);
        }
    }
}
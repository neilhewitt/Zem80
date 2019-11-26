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
        [TestCase("0")]
        [TestCase("1")]
        [TestCase("2")]
        public void IM(int mode)
        {
            Execute($"IM {mode}");
            Assert.That(_cpu.InterruptMode == (InterruptMode)mode && _cpu.InterruptsEnabled);
        }
    }
}
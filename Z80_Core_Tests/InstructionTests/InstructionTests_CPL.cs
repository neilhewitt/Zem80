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
            byte value = RandomByte();
            byte result = value ^= value;

            Registers.A = value;
            Execute("CPL");

            Assert.That(Registers.A == result && TestFlags(halfCarry: true, subtract: true));
        }
    }
}

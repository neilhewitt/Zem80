using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_POP : InstructionTestBase
    {
        [Test]
        [TestCaseSource(typeof(TestCases), "GetRegisterPairs_PUSH_POP")]
        public void POP_rr(RegisterPair registerPair)
        {
            ushort value = RandomWord();
            ushort stackAddress = Registers.SP;
            WriteWordAt(stackAddress, value);

            Execute($"POP { registerPair }");

            Assert.That(Registers[registerPair] == value && Registers.SP == stackAddress + 2);
        }
    }
}
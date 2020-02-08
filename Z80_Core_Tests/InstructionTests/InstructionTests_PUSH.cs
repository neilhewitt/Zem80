using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_PUSH : InstructionTestBase
    {
        [Test]
        [TestCaseSource(typeof(TestCases), "GetRegisterPairs_PUSH_POP")]
        public void PUSH_rr(RegisterPairIndex registerPair)
        {
            ushort value = RandomWord();
            ushort stackAddress = Registers.SP;
            Registers[registerPair] = value; 

            Execute($"PUSH { registerPair }");

            Assert.That(WordAt(stackAddress) == value && Registers.SP == stackAddress - 2);
        }
    }
}
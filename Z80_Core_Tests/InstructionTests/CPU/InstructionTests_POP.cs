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
        public void POP_rr(RegisterPairName registerPair)
        {
            ushort value = RandomWord();
            ushort stackAddress = Registers.SP;
            WriteWordAt(stackAddress, value);

            ExecutionResult executionResult = ExecuteInstruction($"POP { registerPair }");

            Assert.That(Registers[registerPair], Is.EqualTo(value));
            Assert.That(Registers.SP, Is.EqualTo(stackAddress + 2));
        }
    }
}
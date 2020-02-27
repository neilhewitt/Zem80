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
        public void POP_rr([Values(
                RegisterPairName.AF,
                RegisterPairName.BC,
                RegisterPairName.DE,
                RegisterPairName.HL,
                RegisterPairName.IX,
                RegisterPairName.IY
            )] RegisterPairName registerPair)
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
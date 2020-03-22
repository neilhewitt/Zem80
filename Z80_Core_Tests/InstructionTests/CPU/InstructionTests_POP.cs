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
                RegisterWord.AF,
                RegisterWord.BC,
                RegisterWord.DE,
                RegisterWord.HL,
                RegisterWord.IX,
                RegisterWord.IY
            )] RegisterWord registerPair)
        {
            ushort value = 0x5000;;
            ushort stackAddress = Registers.SP;
            WriteWordAt(stackAddress, value);

            ExecutionResult executionResult = ExecuteInstruction($"POP { registerPair }");

            Assert.That(Registers[registerPair], Is.EqualTo(value));
            Assert.That(Registers.SP, Is.EqualTo(stackAddress + 2));
        }
    }
}
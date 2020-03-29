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
        public void PUSH_rr([Values(
                WordRegister.AF,
                WordRegister.BC,
                WordRegister.DE,
                WordRegister.HL,
                WordRegister.IX,
                WordRegister.IY
            )] WordRegister registerPair)
        {
            ushort value = 0x5000;;
            ushort stackAddress = Registers.SP;
            Registers[registerPair] = value; 

            ExecutionResult executionResult = ExecuteInstruction($"PUSH { registerPair }");

            Assert.That(ReadWordAt(Registers.SP), Is.EqualTo(value));
            Assert.That(Registers.SP, Is.EqualTo(stackAddress - 2));
        }
    }
}
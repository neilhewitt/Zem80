using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RET_RETI_RETN : InstructionTestBase
    {
        [Test]
        public void RET()
        {
            ushort address = 0x2000;
            Registers.HL = address;
            CPU.Push(RegisterWord.HL);

            ExecutionResult executionResult = ExecuteInstruction("RET");

            Assert.That(Registers.PC, Is.EqualTo(address));
        }

        [Test]
        public void RETI()
        {
            ushort address = 0x2000;
            Registers.HL = address;
            CPU.Push(RegisterWord.HL);

            ExecutionResult executionResult = ExecuteInstruction("RETI");

            Assert.That(Registers.PC, Is.EqualTo(address));
        }

        [Test]
        public void RETN()
        {
            ushort address = 0x2000;
            Registers.HL = address;
            CPU.Push(RegisterWord.HL);

            ExecutionResult executionResult = ExecuteInstruction("RETN");

            Assert.That(Registers.PC, Is.EqualTo(address));
        }

        [TestCase(Condition.Z)]
        [TestCase(Condition.NZ)]
        [TestCase(Condition.C)]
        [TestCase(Condition.NC)]
        [TestCase(Condition.PE)]
        [TestCase(Condition.PO)]
        [TestCase(Condition.M)]
        [TestCase(Condition.P)]
        public void RET(Condition condition)
        {
            ushort address = 0x2000;
            Registers.HL = address;
            CPU.Push(RegisterWord.HL);

            Registers.Flags.SetFromCondition(condition);
            ExecutionResult executionResult = ExecuteInstruction($"RET { condition }");

            Assert.That(Registers.PC, Is.EqualTo(address));
            Assert.That(Registers.Flags.SatisfyCondition(condition), Is.True);
        }
    }
}
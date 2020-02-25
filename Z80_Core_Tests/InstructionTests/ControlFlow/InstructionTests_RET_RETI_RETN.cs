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
            ushort address = RandomWord(32768);
            Registers.HL = address;
            CPU.Push(RegisterPairName.HL);

            ExecutionResult executionResult = ExecuteInstruction("RET");

            Assert.That(Registers.PC == address);
        }

        [Test]
        public void RETI()
        {
            ushort address = RandomWord(32768);
            Registers.HL = address;
            CPU.Push(RegisterPairName.HL);

            ExecutionResult executionResult = ExecuteInstruction("RETI");

            Assert.That(Registers.PC == address);
        }

        [Test]
        public void RETN()
        {
            ushort address = RandomWord(32768);
            Registers.HL = address;
            CPU.Push(RegisterPairName.HL);

            ExecutionResult executionResult = ExecuteInstruction("RETN");

            Assert.That(Registers.PC == address);
        }

        [TestCase(ConditionFlagName.Z)]
        [TestCase(ConditionFlagName.NZ)]
        [TestCase(ConditionFlagName.C)]
        [TestCase(ConditionFlagName.NC)]
        [TestCase(ConditionFlagName.PE)]
        [TestCase(ConditionFlagName.PO)]
        [TestCase(ConditionFlagName.M)]
        [TestCase(ConditionFlagName.P)]
        public void RET(ConditionFlagName condition)
        {
            ushort address = RandomWord(32768);
            Registers.HL = address;
            CPU.Push(RegisterPairName.HL);

            Registers.Flags.SetFromCondition(condition);
            ExecutionResult executionResult = ExecuteInstruction($"RET { condition }");

            Assert.That(Registers.PC, Is.EqualTo(address));
            Assert.That(Registers.Flags.SatisfyCondition(condition), Is.True);
        }
    }
}
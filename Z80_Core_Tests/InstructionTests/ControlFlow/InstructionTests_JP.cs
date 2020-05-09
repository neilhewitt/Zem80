using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_JP : InstructionTestBase
    {
        [TestCase(Condition.Z)]
        [TestCase(Condition.NZ)]
        [TestCase(Condition.C)]
        [TestCase(Condition.NC)]
        [TestCase(Condition.PE)]
        [TestCase(Condition.PO)]
        [TestCase(Condition.M)]
        [TestCase(Condition.P)]
        public void JP_cc_nn(Condition condition)
        {
            ushort address = 0x5000;
            SetProgramCounter(0x8000);
            Registers.Flags.SetFromCondition(condition);

            ExecutionResult executionResult = ExecuteInstruction($"JP { condition },nn", arg1: address.LowByte(), arg2: address.HighByte());

            ushort newPC = Registers.PC;

            // if conditional JP fails, then newPC != address
            Assert.That(newPC, Is.EqualTo(address));
        }

        [Test]
        public void JP_nn()
        {
            ushort address = 0x5000;;
            SetProgramCounter(0x5000);

            ExecutionResult executionResult = ExecuteInstruction($"JP nn", arg1: address.LowByte(), arg2: address.HighByte());
            ushort newPC = Registers.PC;

            Assert.That(newPC, Is.EqualTo(address));
        }
    }
}
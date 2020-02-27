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
        [TestCase(ConditionFlagName.Z)]
        [TestCase(ConditionFlagName.NZ)]
        [TestCase(ConditionFlagName.C)]
        [TestCase(ConditionFlagName.NC)]
        [TestCase(ConditionFlagName.PE)]
        [TestCase(ConditionFlagName.PO)]
        [TestCase(ConditionFlagName.M)]
        [TestCase(ConditionFlagName.P)]
        public void JP_cc_nn(ConditionFlagName condition)
        {
            ushort address = 0x5000;
            Registers.PC = 0x8000;
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
            Registers.PC = 0x5000;;

            ExecutionResult executionResult = ExecuteInstruction($"JP nn", arg1: address.LowByte(), arg2: address.HighByte());
            ushort newPC = Registers.PC;

            Assert.That(newPC, Is.EqualTo(address));
        }
    }
}
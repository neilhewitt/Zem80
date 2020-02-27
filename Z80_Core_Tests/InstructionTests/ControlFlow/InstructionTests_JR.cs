﻿using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_JR : InstructionTestBase
    {
        [TestCase(ConditionFlagName.Z)]
        [TestCase(ConditionFlagName.NZ)]
        [TestCase(ConditionFlagName.C)]
        [TestCase(ConditionFlagName.NC)]
        public void JR_cc_o(ConditionFlagName condition)
        {
            sbyte jump = 0x7F;
            Registers.PC = 0x5000;
            ushort address = Registers.PC;

            Registers.Flags.SetFromCondition(condition);
            ExecutionResult executionResult = ExecuteInstruction($"JR { condition },o", arg1:(byte)jump);
            ushort expectedPC = (ushort)((address + jump) + 2);

            Assert.That(Registers.PC, Is.EqualTo(expectedPC));
            Assert.That(Registers.Flags.SatisfyCondition(condition), Is.True);
        }

        [Test]
        public void JR_o([Values(0, 127, -127)] sbyte jump)
        {
            Registers.PC = 0x5000;
            ushort address = Registers.PC;

            ExecutionResult executionResult = ExecuteInstruction($"JR o", arg1: (byte)jump);
            ushort expectedPC = (ushort)((address + jump) + 2);

            Assert.That(Registers.PC, Is.EqualTo(expectedPC));
        }
    }
}
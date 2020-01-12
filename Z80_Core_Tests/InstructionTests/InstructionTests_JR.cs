using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_JR : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetZeroAndCarryConditions")]
        public void JR_cc_o((string condition, Func<IFlags> getFlags, Func<IFlags, bool> test) conditions)
        {
            sbyte jump = (sbyte)RandomByte();
            Registers.PC = RandomWord();
            ushort address = Registers.PC;
            IFlags flags = conditions.getFlags();

            Registers.SetFlags(flags);
            Execute($"JR {conditions.condition},o", arg1:(byte)jump);

            ushort newPC = Registers.PC;

            Assert.That(newPC == ((ushort)(address + jump) + 2)); // add 2 bytes for instruction length
        }

        [Test]
        public void JR_o()
        {
            sbyte jump = (sbyte)RandomByte();
            Registers.PC = RandomWord();
            ushort address = Registers.PC;

            Execute($"JR o", arg1: (byte)jump);

            ushort newPC = Registers.PC;

            Assert.That(newPC == ((ushort)(address + jump) + 2)); // add 2 bytes for instruction length
        }
    }
}
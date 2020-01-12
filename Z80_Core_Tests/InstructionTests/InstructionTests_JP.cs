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
        [Test, TestCaseSource(typeof(TestCases), "GetConditions")]
        public void JP_cc_nn((string condition, Func<IFlags> getFlags, Func<IFlags, bool> test) conditions)
        {
            ushort address = RandomWord();
            Registers.PC = RandomWord();
            IFlags flags = conditions.getFlags();

            Registers.SetFlags(flags);
            Execute($"JP {conditions.condition},nn", arg1: address.LowByte(), arg2: address.HighByte());

            ushort newPC = Registers.PC;

            // if conditional JP fails, then newPC != address
            Assert.That(newPC == address);
        }

        [Test]
        public void JP_nn()
        {
            ushort address = RandomWord();
            Registers.PC = RandomWord();

            Execute($"JP nn", arg1: address.LowByte(), arg2: address.HighByte());
            ushort newPC = Registers.PC;

            Assert.That(newPC == address);
        }
    }
}
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_CALL : InstructionTestBase
    {
        [Test]
        public void CALL()
        {
            ushort address = RandomWord();
            Registers.PC = RandomWord();
            ushort pc = Registers.PC;

            Execute("CALL nn", arg1: address.LowByte(), arg2: address.HighByte());

            ushort checkPC = (ushort)(_cpu.Memory.Stack.Pop() - 3); // remove bytes for the CALL instruction itself
            ushort newPC = Registers.PC;

            Assert.That(checkPC == pc && newPC == address);
        }

        [Test, TestCaseSource(typeof(TestCases), "GetConditions")]
        public void CALL_cc_nn((string condition, Func<IFlags> getFlags, Func<IFlags, bool> test) conditions)
        {
            ushort address = RandomWord();
            Registers.PC = RandomWord();
            ushort pc = Registers.PC;
            IFlags flags = conditions.getFlags();

            Registers.SetFlags(flags);
            Execute($"CALL {conditions.condition},nn", arg1: address.LowByte(), arg2: address.HighByte());

            ushort oldPC = (ushort)(_cpu.Memory.Stack.Pop() - 3); // remove bytes for the CALL instruction itself
            ushort newPC = Registers.PC;

            // if conditional CALL fails, then newPC != address
            Assert.That(newPC == address && oldPC == pc);
        }
    }
}

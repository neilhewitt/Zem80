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
            ushort expectedPCAddress = 0x3000;
            ushort originalPCAddress = 0x5000;

            Registers.PC = originalPCAddress;
            ExecutionResult executionResult = ExecuteInstruction("CALL nn", arg1: expectedPCAddress.LowByte(), arg2: expectedPCAddress.HighByte());

            ushort stackStoredPCAddress = (ushort)(CPU.Peek() - 3); ; // peek current stack value and remove 3 bytes for the length of the CALL instruction itself
            ushort finalPCAddress = Registers.PC;

            Assert.That(stackStoredPCAddress, Is.EqualTo(originalPCAddress));
            Assert.That(finalPCAddress, Is.EqualTo(expectedPCAddress));
        }

        [TestCase(Condition.Z)]
        [TestCase(Condition.NZ)]
        [TestCase(Condition.C)]
        [TestCase(Condition.NC)]
        [TestCase(Condition.PE)]
        [TestCase(Condition.PO)]
        [TestCase(Condition.M)]
        [TestCase(Condition.P)]
        public void CALL_cc_nn(Condition condition)
        {
            ushort expectedPCAddress = 0x3000;
            ushort originalPCAddress = 0x5000;
            
            Registers.PC = originalPCAddress;
            Registers.Flags.SetFromCondition(condition);
            ExecutionResult executionResult = ExecuteInstruction($"CALL { condition },nn", arg1: expectedPCAddress.LowByte(), arg2: expectedPCAddress.HighByte());

            ushort stackStoredPCAddress = (ushort)(CPU.Peek() - 3); ; // peek current stack value and remove 3 bytes for the length of the CALL instruction itself
            ushort finalPCAddress = Registers.PC;

            Assert.That(stackStoredPCAddress, Is.EqualTo(originalPCAddress));
            Assert.That(finalPCAddress, Is.EqualTo(expectedPCAddress));
        }
    }
}

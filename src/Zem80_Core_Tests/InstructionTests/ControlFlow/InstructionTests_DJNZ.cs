using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_DJNZ : InstructionTestBase
    {
        [Test]
        public void DJNZ([Values(0x00, 0x01, 0x50, 0x6F, -0x01, -0x80)] sbyte jump)
        {
            ushort address = 0x5000;
            SetProgramCounter(address);
            Registers.B = 1;
            if (jump == 0x6F) Registers.B = 0; // jump will not occur if value in B is zero (the NZ in DJNZ refers to the value of B, *not* the Zero flag)

            ExecutionResult executionResult = ExecuteInstruction("DJNZ o", arg1: (byte)jump);
            ushort expectedAddress = jump switch
            {
                var j when (j == 0x6F) => (ushort)(address + 2), // if jump == 0x6F then Registers.B was set to zero, so jump should not have happened and the program counter should be address + 2 bytes for instruction length
                _ => (ushort)(address + jump + 2) // otherwise, add/subtract the jump from the address
            };

            Assert.That(Registers.PC, Is.EqualTo(expectedAddress));
            Assert.That(Registers.B, Is.Zero); // should be decremented from 1 after instruction, unless it was already zero
        }
    }
}
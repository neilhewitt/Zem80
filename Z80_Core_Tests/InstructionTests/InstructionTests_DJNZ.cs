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
        [TestCase(1)]
        [TestCase(0)]
        public void DJNZ(byte b)
        {
            Registers.B = b;
            sbyte jump = (sbyte)RandomByte();
            ushort PC = RandomWord();
            Registers.PC = PC;

            Execute("DJNZ o", arg1: (byte)jump);

            Assert.That((b != 0) ? 
                (Registers.PC == PC + jump + 2) : 
                Registers.PC == PC);
        }
    }
}
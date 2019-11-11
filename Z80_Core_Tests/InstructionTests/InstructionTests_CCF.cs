using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    public class InstructionTests_CCF : InstructionTestBase
    {
        [Test]
        public void CCF()
        {
            bool halfCarry = RandomBool();
            bool carry = RandomBool();

            Flags flags = new Flags() { HalfCarry = halfCarry, Carry = carry };
            Registers.SetFlags(flags);

            Execute("CCF");

            Assert.That(Registers.Flags.HalfCarry == halfCarry && Registers.Flags.Carry == !carry);
        }
    }
}

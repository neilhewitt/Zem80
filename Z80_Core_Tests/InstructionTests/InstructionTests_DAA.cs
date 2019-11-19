using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_DAA : InstructionTestBase
    {
        [Test]
        public void DAA_HalfCarry()
        {
            byte value = 0x6B; // 0110 (6) 1011 (BCD overflow)

            Registers.A = value;
            Flags.HalfCarry = true;
            Execute("DAA");

            Assert.That(Registers.A == 0x71 && TestFlags(zero: false, sign: false, parityOverflow: false));
        }

        [Test]
        public void DAA_Carry()
        {
            byte value = 0xB6; // 1011 (BCD overflow) 0110 (6)

            Registers.A = value;
            Flags.Carry = true;
            Execute("DAA");

            Assert.That(Registers.A == 0x16 && TestFlags(carry:true, zero: false, sign: false, parityOverflow: true));
        }

        [Test]
        public void DAA_LowOverflow()
        {
            byte value = 0x3F; // 0011 (3) 1111 (BCD overflow)

            Registers.A = value;
            Execute("DAA");

            Assert.That(Registers.A == 0x45 && TestFlags(carry: false, zero: false, sign: false, parityOverflow: false));
        }

        [Test]
        public void DAA_HighOverflow()
        {
            byte value = 0xF3; // 1111 (BCD overflow) 0011 (3) 

            Registers.A = value;
            Execute("DAA");

            Assert.That(Registers.A == 0x53 && TestFlags(carry: true, zero: false, sign: false, parityOverflow: true));
        }
    }
}
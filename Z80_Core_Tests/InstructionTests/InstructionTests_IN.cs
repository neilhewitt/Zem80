using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_IN : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void IN_r_xnC(Register register)
        {
            PortSignal signal;
            byte data = RandomByte();

            Func<byte> reader = () => data;
            Action<PortSignal> signaller = (s) => { signal = s; };
            _cpu.Ports[0].Connect(reader, null, signaller);

            string from = register == Register.A ? "(n)" : "(C)";
            Execute($"IN {register},{from}", registerIndex:register);

            if (register == Register.A)
            {
                Assert.That(Registers.A == data);
            }
            else
            {
                bool sign = (sbyte)data < 0;
                bool zero = data == 0;
                bool parity = data.CountBits(true) % 2 == 0;
                Assert.That(Registers[register] == data &&
                    TestFlags(sign: sign, zero: zero, parityOverflow: parity));
            }
        }
    }
}
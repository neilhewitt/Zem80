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
        public void IN_r_xnC(RegisterIndex register)
        {
            PortSignal signal;
            byte data = RandomByte();
            Func<byte> reader = () => data;
            Action<byte> writer = null;
            Action<PortSignal> signaller = (s) => { signal = s; };
            _cpu.Ports[0].Connect(reader, writer, signaller);

            string from = register == RegisterIndex.A ? "(n)" : "(C)";
            Execute($"IN {register},{from}", registerIndex:register);

            if (register == RegisterIndex.A)
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
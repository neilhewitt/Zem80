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
        [Test]
        public void IN_r_xn()
        {
            byte data = 0x23;

            Func<byte> reader = () => 0x23;
            Action signaller = () => { };
            CPU.Ports[0x00].Connect(reader, null, signaller, null);

            ExecutionResult executionResult = ExecuteInstruction($"IN A,(n)", arg1: 0x00);

            Assert.That(Registers.A, Is.EqualTo(data));
        }

        [Test]
        public void IN_r_xC()
        {
            byte data = 0x23;

            Func<byte> reader = () => data;
            Action signaller = () => { };
            CPU.Ports[0].Connect(reader, null, null, signaller);

            ExecutionResult executionResult = ExecuteInstruction($"IN B,(C)");

            bool sign = (sbyte)data < 0;
            bool zero = data == 0;
            bool parity = data.CountBits(true) % 2 == 0;

            Assert.That(Registers.B, Is.EqualTo(data));
            Assert.That(executionResult.Flags.Check(
                    sign: sign,
                    zero: zero,
                    parityOverflow: parity
                    ),
                Is.True);
        }
    }
}
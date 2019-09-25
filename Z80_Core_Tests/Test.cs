using System;
using NUnit.Framework;
using Z80;

namespace Z80.Core.Tests
{ 
    [TestFixture]
    public class Test
    {
        [Test]
        public void CanSetAndRead8BitRegisterValue()
        {
            Processor cpu = new Processor();
            cpu.Registers.A = 128;
            Assert.That(cpu.Registers.A == 128);
        }

        [Test]
        public void CanSetAndRead16BitRegisterValue()
        {
            Processor cpu = new Processor();
            cpu.Registers.BC = 19123;
            Assert.That(cpu.Registers.BC == 19123);
        }

        [Test]
        public void CanSet16BitRegisterValueAndReadTwo8BitValues()
        {
            Processor cpu = new Processor();
            cpu.Registers.BC = 31874;
            byte[] bytes = BitConverter.GetBytes(31874);
            Assert.That(cpu.Registers.BC == 31874 && cpu.Registers.B == bytes[0] && cpu.Registers.C == bytes[1]);
        }
    }
}

using System;
using System.Linq;
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
            const byte TEST_VALUE = 128;

            Processor cpu = new Processor();
            cpu.Registers.A = TEST_VALUE;

            Assert.That(cpu.Registers.A == TEST_VALUE);
        }

        [Test]
        public void CanSetAndRead16BitRegisterValue()
        {
            const ushort TEST_VALUE = 31724;

            Processor cpu = new Processor();
            cpu.Registers.BC = TEST_VALUE;

            Assert.That(cpu.Registers.BC == TEST_VALUE);
        }

        [Test]
        public void CanSet16BitRegisterValueAndReadTwo8BitValues()
        {
            const ushort TEST_VALUE = 47881;

            Processor cpu = new Processor();
            cpu.Registers.BC = TEST_VALUE;
            byte[] bytes = BitConverter.GetBytes(TEST_VALUE);

            Assert.That(cpu.Registers.BC == TEST_VALUE && cpu.Registers.B == bytes[0] && cpu.Registers.C == bytes[1]);
        }

        [Test]
        public void CanExchangeAFWithAltAFAndBack()
        {
            const byte AF_VALUE = 66;
            const byte ALT_AF_VALUE = 99;

            Processor cpu = new Processor();
            cpu.Registers.A = AF_VALUE;
            cpu.Registers.ExchangeAF();
            cpu.Registers.A = ALT_AF_VALUE;
            cpu.Registers.ExchangeAF();
            byte af = cpu.Registers.A;
            cpu.Registers.ExchangeAF();
            byte altAf = cpu.Registers.A;

            Assert.That(af == AF_VALUE && altAf == ALT_AF_VALUE);
        }

        [Test]
        public void CanExchangeRegistersWithAltRegistersAndBack()
        {
            ushort[] values = new ushort[4];
            ushort[] altValues = new ushort[4];
            ushort[] valuesRecovered = new ushort[4];
            ushort[] altValuesRecovered = new ushort[4];
            Random random = new Random();
            values[0] = (ushort)random.Next(255); // must be a byte value to fit A only
            altValues[0] = (ushort)random.Next(255);
            for (int i = 1; i < 4; i++)
            {
                values[i] = (ushort)random.Next(ushort.MaxValue);
                altValues[i] = (ushort)random.Next(ushort.MaxValue);
            }

            Processor cpu = new Processor();
            cpu.Registers.A = BitConverter.GetBytes(values[0])[0]; // can't set AF directly
            cpu.Registers.BC = values[1];
            cpu.Registers.DE = values[2];
            cpu.Registers.HL = values[3];
           cpu.Registers.ExchangeAll();
            cpu.Registers.A = BitConverter.GetBytes(altValues[0])[0]; // can't set AF directly
            cpu.Registers.BC = altValues[1];
            cpu.Registers.DE = altValues[2];
            cpu.Registers.HL = altValues[3];
            cpu.Registers.ExchangeAll();
            valuesRecovered[0] = cpu.Registers.AF;
            valuesRecovered[1] = cpu.Registers.BC;
            valuesRecovered[2] = cpu.Registers.DE;
            valuesRecovered[3] = cpu.Registers.HL;
            cpu.Registers.ExchangeAll();
            altValuesRecovered[0] = cpu.Registers.AF;
            altValuesRecovered[1] = cpu.Registers.BC;
            altValuesRecovered[2] = cpu.Registers.DE;
            altValuesRecovered[3] = cpu.Registers.HL;

            bool valuesAreCorrect = true;
            for (int i = 0; i < 4; i++)
            {
                if (values[i] != valuesRecovered[i] || altValues[i] != altValuesRecovered[i]) valuesAreCorrect = false;
            }
            Assert.That(valuesAreCorrect);
        }
    }
}

using System;
using System.Linq;
using NUnit.Framework;
using Z80;

namespace Z80.Core.Tests
{ 
    [TestFixture]
    public class RegisterTests
    {
        [Test]
        public void CanSetAndRead8BitRegisterValue()
        {
            const byte TEST_VALUE = 128;

            Registers registers = new Registers();
            registers.A = TEST_VALUE;

            Assert.That(registers.A == TEST_VALUE);
        }

        [Test]
        public void CanSetAndRead16BitRegisterValue()
        {
            const ushort TEST_VALUE = 31724;

            Registers registers = new Registers();
            registers.BC = TEST_VALUE;

            Assert.That(registers.BC == TEST_VALUE);
        }

        [Test]
        public void CanSet16BitRegisterValueAndReadTwo8BitValues()
        {
            const ushort TEST_VALUE = 47881;

            Registers registers = new Registers();
            registers.BC = TEST_VALUE;
            byte[] bytes = BitConverter.GetBytes(TEST_VALUE);

            Assert.That(registers.BC == TEST_VALUE && registers.B == bytes[0] && registers.C == bytes[1]);
        }

        [Test]
        public void CanExchangeAFWithAltAFAndBack()
        {
            const byte AF_VALUE = 66;
            const byte ALT_AF_VALUE = 99;

            Registers registers = new Registers();
            registers.A = AF_VALUE;
            registers.ExchangeAF();
            registers.A = ALT_AF_VALUE;
            registers.ExchangeAF();
            byte af = registers.A;
            registers.ExchangeAF();
            byte altAf = registers.A;

            Assert.That(af == AF_VALUE && altAf == ALT_AF_VALUE);
        }

        [Test]
        public void CanExchangeBCDEHLWithAltBCDEHLAndBack()
        {
            ushort[] values = new ushort[3];
            ushort[] altValues = new ushort[3];
            ushort[] valuesRecovered = new ushort[3];
            ushort[] altValuesRecovered = new ushort[3];
            Random random = new Random();
            for (int i = 0; i < 3; i++)
            {
                values[i] = (ushort)random.Next(ushort.MaxValue);
                altValues[i] = (ushort)random.Next(ushort.MaxValue);
            }

            Registers registers = new Registers();
            registers.BC = values[0];
            registers.DE = values[1];
            registers.HL = values[2];
            registers.ExchangeBCDEHL();
            registers.BC = altValues[0];
            registers.DE = altValues[1];
            registers.HL = altValues[2];
            registers.ExchangeBCDEHL();
            valuesRecovered[0] = registers.BC;
            valuesRecovered[1] = registers.DE;
            valuesRecovered[2] = registers.HL;
            registers.ExchangeBCDEHL();
            altValuesRecovered[0] = registers.BC;
            altValuesRecovered[1] = registers.DE;
            altValuesRecovered[2] = registers.HL;

            bool valuesAreCorrect = true;
            for (int i = 0; i < 3; i++)
            {
                if (values[i] != valuesRecovered[i] || altValues[i] != altValuesRecovered[i]) valuesAreCorrect = false;
            }
            Assert.That(valuesAreCorrect);
        }

        [Test]
        public void CanObtainRegisterState()
        {
            ushort TEST_VALUE = 37268;

            Registers registers = new Registers();
            registers.BC = TEST_VALUE;
            registers.HL = TEST_VALUE;

            Registers state = registers.Snapshot();
            Assert.That(state.BC == TEST_VALUE && state.HL == TEST_VALUE);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Zem80.Core.Tests.MicrocodeTests
{
    [TestFixture]
    public class InputOutputTests : MicrocodeTestBase
    {
        [Test]
        public void IND()
        {
            byte input = 0x1E;

            CPU.Ports[0xFF].Connect(() => input, null, null, null);
            Registers.C = 0xFF;
            Registers.B = 0x02;
            Registers.HL = 0x5000;

            ExecuteInstruction("IND");

            Assert.That(
                Registers.B == 0x01 &&
                CPU.Memory.ReadByteAt(0x5000) == input &&
                Registers.HL == 0x4FFF && 
                // IN r,(C) sets the flags, unlike the other IN / OUT instructions
                Flags.Zero == false &&
                Flags.Subtract == true &&
                Flags.X == true &&
                Flags.Y == false
                );
        }

        [Test]
        public void INI()
        {
            byte input = 0x1E;

            CPU.Ports[0xFF].Connect(() => input, null, null, null);
            Registers.C = 0xFF;
            Registers.B = 0x02;
            Registers.HL = 0x5000;

            ExecuteInstruction("INI");

            Assert.That(
                Registers.B == 0x01 &&
                CPU.Memory.ReadByteAt(0x5000) == input &&
                Registers.HL == 0x5001 &&
                // IN r,(C) sets the flags, unlike the other IN / OUT instructions
                Flags.Zero == false &&
                Flags.Subtract == true &&
                Flags.X == true &&
                Flags.Y == false
                );
        }

        [Test]
        public void INDR()
        {
            byte input = 0x1E;
            ushort pc = Registers.PC;

            CPU.Ports[0xFF].Connect(() => input, null, null, null);
            Registers.C = 0xFF;
            Registers.B = 0x02;
            Registers.HL = 0x5000;

            ExecuteInstruction("INDR");

            Assert.That(
                Registers.B == 0x01 &&
                CPU.Memory.ReadByteAt(0x5000) == input &&
                Registers.HL == 0x4FFF &&
                Registers.PC == pc && 
                Flags.Zero == true &&
                Flags.Subtract == true &&
                Flags.X == true &&
                Flags.Y == false
                );
        }

        [Test]
        public void INIR()
        {
            byte input = 0x1E;
            ushort pc = Registers.PC;

            CPU.Ports[0xFF].Connect(() => input, null, null, null);
            Registers.C = 0xFF;
            Registers.B = 0x02;
            Registers.HL = 0x5000;

            ExecuteInstruction("INIR");

            Assert.That(
                Registers.B == 0x01 &&
                CPU.Memory.ReadByteAt(0x5000) == input &&
                Registers.HL == 0x5001 &&
                Registers.PC == pc &&
                Flags.Zero == true &&
                Flags.Subtract == true &&
                Flags.X == true &&
                Flags.Y == false
                );
        }

        [Test]
        public void IN_A_n()
        {
            byte input = 0xD1;

            CPU.Ports[0].Connect(() => input, null, null, null);
            ExecuteInstruction("IN A,(n)", 0x00);

            Assert.That(Registers.A == input);  
        }

        [Test]
        public void IN_r_C()
        {
            byte input = 0x1E;

            CPU.Ports[0xFF].Connect(() => input, null, null, null);
            Registers.C = 0xFF;

            ExecuteInstruction("IN B,(C)", 0x00);
            Assert.That(
                Registers.B == input &&
                Flags.Sign == false &&
                Flags.Zero == false &&
                Flags.ParityOverflow == true &&
                Flags.HalfCarry == false &&
                Flags.Subtract == false &&
                Flags.X == true &&
                Flags.Y == false
                );
        }

        [Test]
        public void OUTD()
        {
            byte output = 0x00;

            CPU.Ports[0x00].Connect(null, (o) => output = o, null, null);
            Registers.C = 0x00;
            Registers.B = 0x02;
            Registers.HL = 0x5000;
            CPU.Memory.WriteByteAt(Registers.HL, 0x55);

            ExecuteInstruction("OUTD");

            Assert.That(
                Registers.B == 0x01 &&
                CPU.Memory.ReadByteAt(0x5000) == output &&
                Registers.HL == 0x4FFF &&
                Flags.Zero == false &&
                Flags.Subtract == true &&
                Flags.X == false &&
                Flags.Y == false
                );
        }

        [Test]
        public void OUTI()
        {
            byte output = 0x00;

            CPU.Ports[0x00].Connect(null, (o) => output = o, null, null);
            Registers.C = 0x00;
            Registers.B = 0x02;
            Registers.HL = 0x5000;
            CPU.Memory.WriteByteAt(Registers.HL, 0x55);

            ExecuteInstruction("OUTI");

            Assert.That(
                Registers.B == 0x01 &&
                CPU.Memory.ReadByteAt(0x5000) == output &&
                Registers.HL == 0x5001 &&
                Flags.Zero == false &&
                Flags.Subtract == true &&
                Flags.X == false &&
                Flags.Y == false
                );
        }

        [Test]
        public void OTDR()
        {
            byte output = 0x00;
            ushort pc = Registers.PC;

            CPU.Ports[0x00].Connect(null, (o) => output = o, null, null);
            Registers.C = 0x00;
            Registers.B = 0x02;
            Registers.HL = 0x5000;
            CPU.Memory.WriteByteAt(Registers.HL, 0x55);

            ExecuteInstruction("OTDR");

            Assert.That(
                Registers.B == 0x01 &&
                CPU.Memory.ReadByteAt(0x5000) == output &&
                Registers.HL == 0x4FFF &&
                Registers.PC == pc && 
                Flags.Zero == true &&
                Flags.Subtract == true &&
                Flags.X == false &&
                Flags.Y == false
                );
        }

        [Test]
        public void OTIR()
        {
            byte output = 0x00;
            ushort pc = Registers.PC;

            CPU.Ports[0x00].Connect(null, (o) => output = o, null, null);
            Registers.C = 0x00;
            Registers.B = 0x02;
            Registers.HL = 0x5000;
            CPU.Memory.WriteByteAt(Registers.HL, 0x55);

            ExecuteInstruction("OTIR");

            Assert.That(
                Registers.B == 0x01 &&
                CPU.Memory.ReadByteAt(0x5000) == output &&
                Registers.HL == 0x5001 &&
                Registers.PC == pc &&
                Flags.Zero == true &&
                Flags.Subtract == true &&
                Flags.X == false &&
                Flags.Y == false
                );
        }

        [Test]
        public void OUT_n_A()
        {
            byte output = 0x00;
            Registers.A = 0x24;

            CPU.Ports[0xE4].Connect(null, (o) => output = o, null, null);
            ExecuteInstruction("OUT (n),A", 0xE4);

            Assert.That(output == Registers.A);
        }

        [Test]
        public void OUT_C_r()
        {
            byte output = 0x00;
            Registers.B = 0x44;

            CPU.Ports[0xFF].Connect(null, (o) => output = o, null, null);
            Registers.C = 0xFF;

            ExecuteInstruction("OUT (C),B");
            Assert.That(output == Registers.B);
        }
    }
}

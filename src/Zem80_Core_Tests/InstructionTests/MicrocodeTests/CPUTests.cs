using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Zem80.Core.Tests.MicrocodeTests
{
    [TestFixture]
    public class CPUTests : MicrocodeTestBase
    {
        [Test]
        public void SCF()
        {
            ExecuteInstruction("SCF");
            Assert.That(Flags.Carry && !Flags.Subtract && !Flags.HalfCarry);
        }

        [Test]
        public void CCF()
        {
            Flags.Carry = true;
            ExecuteInstruction("CCF");
            Assert.That(!Flags.Carry && !Flags.Subtract && Flags.HalfCarry);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void IM(int mode)
        {
            ExecuteInstruction($"IM {mode}");
            Assert.That(CPU.InterruptMode == (InterruptMode)mode);
        }

        [Test]
        public void PUSH()
        {
            ushort valueToPush = 0xF1D0;
            Registers.BC = valueToPush;
            Registers.SP = 0xFFFD;

            ExecuteInstruction("PUSH BC");
            Assert.That(Registers.SP == 0xFFFB && CPU.Memory.Untimed.ReadWordAt(Registers.SP) == valueToPush);
        }

        [Test]
        public void POP()
        {
            ushort valueToPop = 0x1F0D;
            Registers.SP = 0xFFFB;
            CPU.Memory.Untimed.WriteWordAt(Registers.SP, valueToPop);

            ExecuteInstruction("POP DE");
            Assert.That(Registers.SP == 0xFFFD && Registers.DE == valueToPop);
        }

        [Test]
        public void EX_AF_AF()
        {
            Registers.AF = 0x80;
            Registers.ExchangeAF();
            Registers.AF = 0x90;
            Registers.ExchangeAF();

            ExecuteInstruction("EX AF,AF'");
            Assert.That(CPU.Registers.AF == 0x90);

            ExecuteInstruction("EX AF,AF'");
            Assert.That(CPU.Registers.AF == 0x80);
        }

        [TestCase(WordRegister.HL)]
        [TestCase(WordRegister.IX)]
        [TestCase(WordRegister.IY)]
        public void EX_xSP_rr(WordRegister wordRegister)
        {
            ushort sp = 0x4000;
            ushort value = 0x8020;
            ushort valueAtSP = 0x2080;

            Registers.SP = sp;
            CPU.Memory.Untimed.WriteWordAt(sp, valueAtSP);
            Registers[wordRegister] = value;

            ExecuteInstruction($"EX (SP),{ wordRegister.ToString() }");
            ushort newValueAtSP = CPU.Memory.Untimed.ReadWordAt(sp);

            Assert.That(Registers[wordRegister] == valueAtSP && newValueAtSP == value);
        }

        [Test]
        public void EX_DE_HL()
        {
            Registers.DE = 0x80;
            Registers.HL = 0x90;

            ExecuteInstruction("EX DE,HL");
            Assert.That(Registers.HL == 0x80 && Registers.DE == 0x90);
        }
    }
}

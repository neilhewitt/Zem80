using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_EX_X : InstructionTestBase
    {
        [Test]
        public void EX_AF_AF()
        {
            ushort AF = 0x7F00;
            ushort altAF = 0xFFFF;

            ((IDebugRegisters)Registers).AF = AF;
            ExecuteInstruction("EX AF,AF'");
            ((IDebugRegisters)Registers).AF = altAF;
            ExecuteInstruction("EX AF,AF'");
            ushort firstAF = Registers.AF;
            ExecuteInstruction("EX AF,AF'");
            ushort secondAF = Registers.AF;

            Assert.That(firstAF, Is.EqualTo(AF));
            Assert.That(secondAF, Is.EqualTo(altAF));
        }

        [Test]
        public void EX_xSP_HL()
        {
            ushort bcWord = 0x8000;
            Registers.BC = bcWord;
            CPU.Push(RegisterWord.BC); // push value of BC on the stack

            ushort hlWord = 0x5000;
            Registers.HL = hlWord;

            ExecuteInstruction("EX (SP),HL");
            CPU.Pop(RegisterWord.BC); // pop stack value into BC - should be previous value of HL, and HL should be previous value of BC, via the stack

            Assert.That(Registers.HL, Is.EqualTo(bcWord));
            Assert.That(Registers.BC, Is.EqualTo(hlWord));
        }

        [Test]
        public void EX_DE_HL()
        {
            ushort deWord = 0x8000;
            ushort hlWord = 0x5000;
            Registers.DE = deWord;
            Registers.HL = hlWord;

            ExecuteInstruction("EX DE,HL");

            Assert.That(Registers.DE, Is.EqualTo(hlWord));
            Assert.That(Registers.HL, Is.EqualTo(deWord));
        }

        [Test]
        public void EX_xSP_Index([Values(RegisterWord.IX, RegisterWord.IY)] RegisterWord indexRegister)
        {
            ushort indexWord = 0x8000;
            Registers[indexRegister] = indexWord;

            ushort bcWord = 0x5000;
            Registers.BC = bcWord;
            CPU.Push(RegisterWord.BC);

            ExecuteInstruction($"EX (SP),{indexRegister}");
            CPU.Pop(RegisterWord.BC); // pop stack value into BC - should be previous value of IX/IY, and IX/IY should be previous value of BC, via the stack

            Assert.That(Registers[indexRegister], Is.EqualTo(bcWord));
            Assert.That(Registers.BC, Is.EqualTo(indexWord));
        }

        [Test]
        public void EXX()
        {
            ushort BC = 0x5000;
            ushort DE = 0x8000;
            ushort HL = 0x2000;

            ushort altBC = 0x3000;
            ushort altDE = 0x7000;
            ushort altHL = 0x9000;

            Registers.BC = BC;
            Registers.DE = DE;
            Registers.HL = HL;

            ExecuteInstruction("EXX");

            Registers.BC = altBC;
            Registers.DE = altDE;
            Registers.HL = altHL;

            ExecuteInstruction("EXX");

            assert(Registers.BC, BC);
            assert(Registers.DE, DE);
            assert(Registers.HL, HL);

            ExecuteInstruction("EXX");

            assert(Registers.BC, altBC);
            assert(Registers.DE, altDE);
            assert(Registers.HL, altHL);

            void assert(ushort first, ushort second)
            {
                Assert.That(first, Is.EqualTo(second));
            }
        }
    }
}
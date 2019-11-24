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
        public void EX_AF_altAF()
        {
            ushort AF = RandomWord();
            ushort altAF = RandomWord();

            Registers.AF = AF;
            Execute("EX AF,AF'");
            Registers.AF = altAF;
            Execute("EX AF,AF'");
            ushort firstAF = Registers.AF;
            Execute("EX AF,AF'");
            ushort secondAF = Registers.AF;

            Assert.That(firstAF == AF && secondAF == altAF);
        }

        [Test]
        public void EX_xSP_HL()
        {
            ushort stackWord = RandomWord();
            _cpu.Stack.Push(stackWord);

            ushort hlWord = RandomWord();
            Registers.HL = hlWord;

            Execute("EX (SP),HL");

            Assert.That(Registers.HL == stackWord && WordAt(Registers.SP) == hlWord);
        }

        [Test]
        public void EX_DE_HL()
        {
            ushort DE = RandomWord();
            ushort HL = RandomWord();
            Registers.DE = DE;
            Registers.HL = HL;

            Execute("EX DE,HL");

            Assert.That(Registers.DE == HL && Registers.HL == DE);
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void EX_xSP_Index(RegisterPairIndex indexRegister)
        {
            ushort stackWord = RandomWord();
            _cpu.Stack.Push(stackWord);

            ushort indexWord = RandomWord();
            Registers[indexRegister] = indexWord;

            Execute($"EX (SP),{indexRegister}");

            Assert.That(Registers[indexRegister] == stackWord && WordAt(Registers.SP) == indexWord);
        }

        [Test]
        public void EXX()
        {
            ushort BC = RandomWord();
            ushort DE = RandomWord();
            ushort HL = RandomWord();

            ushort altBC = RandomWord();
            ushort altDE = RandomWord();
            ushort altHL = RandomWord();

            Registers.BC = BC;
            Registers.DE = DE;
            Registers.HL = HL;

            Execute("EXX");

            Registers.BC = altBC;
            Registers.DE = altDE;
            Registers.HL = altHL;

            Execute("EXX");

            Assert.That(Registers.BC == BC && Registers.DE == DE && Registers.HL == HL);

            Execute("EXX");

            Assert.That(Registers.BC == altBC && Registers.DE == altDE && Registers.HL == altHL);
        }
    }
}
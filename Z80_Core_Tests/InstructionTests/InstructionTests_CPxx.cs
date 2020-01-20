using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_CPxx : InstructionTestBase
    {
        [Test]
        public void CPD()
        {
            byte first = RandomByte();
            byte second = RandomByte();
            ushort address = RandomWord(0xFFF7);
            ushort byteCount = RandomWord();
            short result = (short)(first - second);

            bool zero = (result == 0);
            bool sign = ((sbyte)result < 0);
            bool halfCarry = first.HalfCarryWhenSubtracting(second);
            bool parityOverflow = byteCount == 1; // will become 0
            bool carry = (result > 0xFF);

            Registers.A = first;
            Registers.HL = address;
            Registers.BC = byteCount;
            WriteByteAt(Registers.HL, second);

            Execute($"CPD");

            Assert.That(Registers.HL == (address - 1) && Registers.BC == (byteCount - 1) 
                && TestFlags(zero: zero, sign: sign, halfCarry: halfCarry, parityOverflow: parityOverflow, carry: carry));
        }

        [Test]
        public void CPI()
        {
            byte first = RandomByte();
            byte second = RandomByte();
            ushort address = RandomWord(0xFFF7);
            ushort byteCount = RandomWord();
            short result = (short)(first - second);

            bool zero = (result == 0);
            bool sign = ((sbyte)result < 0);
            bool halfCarry = first.HalfCarryWhenSubtracting(second);
            bool parityOverflow = byteCount == 1; // will become 0
            bool carry = (result > 0xFF);

            Registers.A = first;
            Registers.HL = address;
            Registers.BC = byteCount;
            WriteByteAt(Registers.HL, second);

            Execute($"CPI");

            Assert.That(Registers.HL == (address + 1) && Registers.BC == (byteCount - 1)
                && TestFlags(zero: zero, sign: sign, halfCarry: halfCarry, parityOverflow: parityOverflow, carry: carry));
        }

        [Test]
        public void CPDR()
        {
            byte first = RandomByte();
            ushort address = RandomWord(0xFFF7);
            ushort byteCount = RandomWord(0x7F); // keep loop size down
            bool findInRange = RandomByte() < 0x7F;

            bool zero, sign, halfCarry, parityOverflow;

            Registers.A = first;
            Registers.HL = address;
            Registers.BC = byteCount;
            WriteByteAt((ushort)(address - (findInRange ? byteCount - 1 : byteCount + 1)), first);

            // we need to simulate the loop processing the CPU does here
            while (!Registers.Flags.Zero && Registers.BC > 0)
            {
                byte second = _cpu.Memory.ReadByteAt(Registers.HL);
                short result = (short)(first - second);
                zero = (result == 0);
                sign = ((sbyte)result < 0);
                halfCarry = first.HalfCarryWhenSubtracting(second);

                ushort hl = Registers.HL;
                ushort bc = Registers.BC;
                Execute($"CPDR");
                parityOverflow = Registers.BC != 0;

                Assert.That(hl == Registers.HL + 1 && bc == Registers.BC + 1 &&
                    TestFlags(zero: zero, sign: sign, halfCarry: halfCarry, parityOverflow: parityOverflow));
            }

            Assert.That(Registers.Flags.Zero || !Registers.Flags.ParityOverflow);
        }

        [Test]
        public void CPIR()
        {
            byte first = RandomByte();
            ushort address = RandomWord(0xFFF7);
            ushort byteCount = RandomWord(0x7F); // keep loop size down
            bool findInRange = RandomByte() < 0x7F;

            bool zero, sign, halfCarry, parityOverflow;

            Registers.A = first;
            Registers.HL = address;
            Registers.BC = byteCount;
            WriteByteAt((ushort)(address + (findInRange ? byteCount - 1 : byteCount + 1)), first);

            // we need to simulate the loop processing the CPU does here
            while (!Registers.Flags.Zero && Registers.BC > 0)
            {
                byte second = _cpu.Memory.ReadByteAt(Registers.HL);
                short result = (short)(first - second);
                zero = (result == 0);
                sign = ((sbyte)result < 0);
                halfCarry = first.HalfCarryWhenSubtracting(second);

                ushort hl = Registers.HL;
                ushort bc = Registers.BC;
                Execute($"CPIR");
                parityOverflow = Registers.BC != 0;

                Assert.That(hl == Registers.HL - 1 && bc == Registers.BC + 1 &&
                    TestFlags(zero: zero, sign: sign, halfCarry: halfCarry, parityOverflow: parityOverflow));
            }

            Assert.That(Registers.Flags.Zero || !Registers.Flags.ParityOverflow);
        }
    }
}

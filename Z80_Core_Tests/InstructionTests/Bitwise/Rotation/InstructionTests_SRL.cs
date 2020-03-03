using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_SRL : InstructionTestBase
    {
        private Flags GetExpectedFlags(byte original, byte expected, bool carry)
        {
            Flags flags = new Flags();
            flags = FlagLookup.FlagsFromBitwiseOperation(original, BitwiseOperation.ShiftRight);
            flags.HalfCarry = false;
            flags.Subtract = false;
            return flags;
        }

        [Test]
        public void SRL_B()
        {
            byte value = 0x7F;
            byte expected = (byte)(value >> 1);
            Registers.B = value;
            expected = expected.SetBit(7, false);

            ExecutionResult executionResult = ExecuteInstruction($"SRL B");

            Flags expectedFlags = GetExpectedFlags(value, expected, value.GetBit(0));

            Assert.That(Registers.B, Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }


        [Test]
        public void SRL_xHL()
        {
            byte value = 0x7F;
            byte expected = (byte)(value >> 1);
            ushort address = 0x5000;
            WriteByteAt(address, value);
            Registers.HL = address;
            expected = expected.SetBit(7, false);

            ExecutionResult executionResult = ExecuteInstruction($"SRL (HL)");

            Flags expectedFlags = GetExpectedFlags(value, expected, value.GetBit(0));

            Assert.That(ReadByteAt(address), Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SRL_xIndexOffset([Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName indexRegister, [Values(127, -128)] sbyte offset)
        {
            byte value = 0x7F;
            byte expected = (byte)(value >> 1);
            ushort address = 0x5000;
            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;
            expected = expected.SetBit(7, false);

            ExecutionResult executionResult = ExecuteInstruction($"SRL ({ indexRegister }+o)", arg1: (byte)offset);

            Flags expectedFlags = GetExpectedFlags(value, expected, value.GetBit(0));

            Assert.That(ReadByteAtIndexAndOffset(indexRegister, offset), Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }
    }
}
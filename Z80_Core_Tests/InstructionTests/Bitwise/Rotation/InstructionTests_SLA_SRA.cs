using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_SLA_SRA : InstructionTestBase
    {
        private Flags GetExpectedFlags(byte original, byte expected, bool carry)
        {
            Flags flags = new Flags();
            flags.Carry = carry;
            if (((sbyte)expected) < 0) flags.Sign = true;
            if (expected == 0) flags.Zero = true;
            if (expected.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
            return flags;
        }

        [Test]
        public void SLA_A()
        {
            byte value = 0x7F;
            byte expected = (byte)(value << 1);
            Registers.A = value;

            ExecutionResult executionResult = ExecuteInstruction($"SLA A");

            Flags expectedFlags = GetExpectedFlags(value, expected, value.GetBit(7));

            Assert.That(Registers.A, Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SRA_B()
        {
            byte value = 0x7F;
            byte expected = (byte)(value >> 1);
            Registers.B = value;

            ExecutionResult executionResult = ExecuteInstruction($"SRA B");

            Flags expectedFlags = GetExpectedFlags(value, expected, value.GetBit(0));

            Assert.That(Registers.B, Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SLA_xHL()
        {
            byte value = 0x7F;
            byte expected = (byte)(value << 1);
            ushort address = 0x5000;
            WriteByteAt(address, value);
            Registers.HL = address;

            ExecutionResult executionResult = ExecuteInstruction($"SLA (HL)");

            Flags expectedFlags = GetExpectedFlags(value, expected, value.GetBit(7));

            Assert.That(ReadByteAt(address), Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SRA_xHL()
        {
            byte value = 0x7F;
            byte expected = (byte)(value >> 1);
            ushort address = 0x5000;
            WriteByteAt(address, value);
            Registers.HL = address;

            ExecutionResult executionResult = ExecuteInstruction($"SRA (HL)");

            Flags expectedFlags = GetExpectedFlags(value, expected, value.GetBit(0));

            Assert.That(ReadByteAt(address), Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SLA_xIndexOffset([Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName indexRegister, [Values(127, -128)] sbyte offset)
        {
            byte value = 0x7F;
            byte expected = (byte)(value << 1);
            ushort address = 0x5000;
            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;

            ExecutionResult executionResult = ExecuteInstruction($"SLA ({ indexRegister }+o)", arg1: (byte)offset);

            Flags expectedFlags = GetExpectedFlags(value, expected, value.GetBit(7));

            Assert.That(ReadByteAtIndexAndOffset(indexRegister, offset), Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SRA_xIndexOffset([Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName indexRegister, [Values(127, -128)] sbyte offset)
        {
            byte value = 0x7F;
            byte expected = (byte)(value >> 1);
            ushort address = 0x5000;
            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;

            ExecutionResult executionResult = ExecuteInstruction($"SRA ({ indexRegister }+o)", arg1: (byte)offset);

            Flags expectedFlags = GetExpectedFlags(value, expected, value.GetBit(0));

            Assert.That(ReadByteAtIndexAndOffset(indexRegister, offset), Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }
    }
}
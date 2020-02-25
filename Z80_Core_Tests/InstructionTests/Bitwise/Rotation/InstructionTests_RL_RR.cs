using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;
using System.Linq;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RL_RR : InstructionTestBase
    {
        [Test]
        public void RL_A([Values(true, false)] bool carry)
        {
            Flags flags = Registers.Flags;
            flags.Carry = carry;
            byte value = RandomByte(0xE0);
            byte expected = ((byte)(value << 1)).SetBit(0, flags.Carry);
            Registers.A = value; // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RL A");

            flags = GetComparisonFlags(value, expected, 7);

            Assert.That(
                Registers.A == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test]
        public void RR_B([Range(0, 0xFF)] byte value, [Values(true, false)] bool carry)
        {
            Registers.Flags.Carry = carry;
            Registers.B = value; // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RR B");

            byte expected = ((byte)(value >> 1)).SetBit(7, carry);
            Flags flags = GetComparisonFlags(value, expected, 0);

            Assert.That(Registers.B, Is.EqualTo(expected));
            Assert.That(CompareWithCPUFlags(flags), Is.True);
        }

        [Test]
        public void RL_xHL([Values(true, false)] bool carry)
        {
            Flags flags = Registers.Flags;
            flags.Carry = carry;
            byte value = RandomByte(0xE0);
            byte expected = ((byte)(value << 1)).SetBit(0, flags.Carry);

            ushort address = RandomWord();
            WriteByteAt(address, value);
            Registers.HL = address;

            ExecutionResult executionResult = ExecuteInstruction($"RL (HL)");

            flags = GetComparisonFlags(value, expected, 7);

            Assert.That(
                ReadByteAt(address) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test]
        public void RR_xHL([Values(true, false)] bool carry)
        {
            Flags flags = Registers.Flags;
            flags.Carry = carry;
            byte value = RandomByte(0xE0);
            byte expected = ((byte)(value >> 1)).SetBit(7, flags.Carry);

            ushort address = RandomWord();
            WriteByteAt(address, value);
            Registers.HL = address;

            ExecutionResult executionResult = ExecuteInstruction($"RR (HL)");

            flags = GetComparisonFlags(value, expected, 0);

            Assert.That(
                ReadByteAt(address) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void RL_xIndexOffset(RegisterPairName indexRegister)
        {
            Flags flags = Registers.Flags;
            flags.Carry = indexRegister == RegisterPairName.IX;
            byte value = RandomByte(0xE0);
            byte expected = ((byte)(value << 1)).SetBit(0, flags.Carry);

            ushort address = RandomWord();
            sbyte offset = (sbyte)RandomByte();
            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;

            ExecutionResult executionResult = ExecuteInstruction($"RL ({ indexRegister }+o)", arg1: (byte)offset);

            flags = GetComparisonFlags(value, expected, 7);

            Assert.That(
                ReadByteAt((ushort)(address + offset)) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void RR_xIndexOffset(RegisterPairName indexRegister)
        {
            Flags flags = Registers.Flags;
            flags.Carry = indexRegister == RegisterPairName.IY;
            byte value = RandomByte(0xE0);
            byte expected = ((byte)(value >> 1)).SetBit(7, flags.Carry);

            ushort address = RandomWord();
            sbyte offset = (sbyte)RandomByte();

            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;

            ExecutionResult executionResult = ExecuteInstruction($"RR ({ indexRegister }+o)", arg1: (byte)offset);

            flags = GetComparisonFlags(value, expected, 0);

            Assert.That(
                ReadByteAt((ushort)(address + offset)) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        private Flags GetComparisonFlags(byte original, byte expected, int bitIndex)
        {
            Flags flags = new Flags();
            flags.Carry = original.GetBit(bitIndex);
            if (((sbyte)expected) < 0) flags.Sign = true;
            if (expected == 0) flags.Zero = true;
            if (expected.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
            return flags;
        }
    }
}
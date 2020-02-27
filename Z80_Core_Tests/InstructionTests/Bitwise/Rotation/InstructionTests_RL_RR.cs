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
        private Flags GetExpectedFlags(byte original, byte expected, int bitIndex)
        {
            Flags flags = new Flags();
            flags.Carry = original.GetBit(bitIndex);
            if (((sbyte)expected) < 0) flags.Sign = true;
            if (expected == 0) flags.Zero = true;
            if (expected.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
            return flags;
        }

        [Test]
        public void RL_A([Values(0x00, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            Registers.Flags.Carry = carry;
            Registers.A = input; // single branch of code, no need to test all registers

            ExecutionResult executionResult = ExecuteInstruction($"RL A");
            byte expected = ((byte)(input << 1)).SetBit(0, carry);
            Flags expectedFlags = GetExpectedFlags(input, expected, 7);

            Assert.That(Registers.A, Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void RR_B([Values(0x00, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            Registers.Flags.Carry = carry;
            Registers.B = input;

            ExecutionResult executionResult = ExecuteInstruction($"RR B");
            byte expected = ((byte)(input >> 1)).SetBit(7, carry);
            Flags expectedFlags = GetExpectedFlags(input, expected, 0);

            Assert.That(Registers.B, Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
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

            flags = GetExpectedFlags(value, expected, 7);

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

            flags = GetExpectedFlags(value, expected, 0);

            Assert.That(
                ReadByteAt(address) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetIndexRegisters")]
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

            flags = GetExpectedFlags(value, expected, 7);

            Assert.That(
                ReadByteAt((ushort)(address + offset)) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetIndexRegisters")]
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

            flags = GetExpectedFlags(value, expected, 0);

            Assert.That(
                ReadByteAt((ushort)(address + offset)) == expected &&
                CompareWithCPUFlags(flags)
                );
        }
    }
}
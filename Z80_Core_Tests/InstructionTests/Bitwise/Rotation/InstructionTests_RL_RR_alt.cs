using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;
using System.Linq;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RL_RR_alt : InstructionTestBase
    {
        [Test]
        public void RL_A()
        {
            // 0x8F -> 0x1E
            // carry: false -> true

            Flags flags = Registers.Flags;
            flags.Carry = false;
            byte value = 0x8F;
            byte expected = 0x1E;
            Registers.A = value;

            ExecutionResult executionResult = ExecuteInstruction($"RL A");

            flags = GetFlags(value, expected, true);

            Assert.That(
                Registers.A == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test]
        public void RR_B()
        {
            // 0xDD -> 0x6E
            // carry: false -> true

            Flags flags = Registers.Flags;
            flags.Carry = false;
            byte value = 0xDD;
            byte expected = 0x6E;
            Registers.B = value;

            ExecutionResult executionResult = ExecuteInstruction($"RR B");

            flags = GetFlags(value, expected, true);

            Assert.That(
                Registers.B == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test]
        public void RL_xHL()
        {
            // 0x8F -> 0x1E
            // carry: false -> true

            Flags flags = Registers.Flags;
            flags.Carry = false;
            byte value = 0x8F;
            byte expected = 0x1E;

            ushort address = RandomWord();
            WriteByteAt(address, value);
            Registers.HL = address;

            ExecutionResult executionResult = ExecuteInstruction($"RL (HL)");

            flags = GetFlags(value, expected, true);

            Assert.That(
                ReadByteAt(address) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test]
        public void RR_xHL()
        {
            // 0xDD -> 0x6E
            // carry: false -> true

            Flags flags = Registers.Flags;
            flags.Carry = false;
            byte value = 0xDD;
            byte expected = 0x6E;

            ushort address = RandomWord();
            WriteByteAt(address, value);
            Registers.HL = address;

            ExecutionResult executionResult = ExecuteInstruction($"RR (HL)");

            flags = GetFlags(value, expected, true);

            Assert.That(
                ReadByteAt(address) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void RL_xIndexOffset(RegisterPairName indexRegister)
        {
            // 0x8F -> 0x1E
            // carry: false -> true

            Flags flags = Registers.Flags;
            flags.Carry = false;
            byte value = 0x8F;
            byte expected = 0x1E;

            ushort address = RandomWord();
            sbyte offset = (sbyte)RandomByte();
            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;

            ExecutionResult executionResult = ExecuteInstruction($"RL ({ indexRegister }+o)", arg1: (byte)offset);

            flags = GetFlags(value, expected, true);

            Assert.That(
                ReadByteAt((ushort)(address + offset)) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void RR_xIndexOffset(RegisterPairName indexRegister)
        {
            // 0xDD -> 0x6E
            // carry: false -> true

            Flags flags = Registers.Flags;
            flags.Carry = false;
            byte value = 0xDD;
            byte expected = 0x6E;

            ushort address = RandomWord();
            sbyte offset = (sbyte)RandomByte();

            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;

            ExecutionResult executionResult = ExecuteInstruction($"RR ({ indexRegister }+o)", arg1: (byte)offset);

            flags = GetFlags(value, expected, true);

            Assert.That(
                ReadByteAt((ushort)(address + offset)) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        private Flags GetFlags(byte original, byte expected, bool carry)
        {
            Flags flags = new Flags();
            flags.Carry = carry;
            if (((sbyte)expected) < 0) flags.Sign = true;
            if (expected == 0) flags.Zero = true;
            if (expected.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
            return flags;
        }
    }
}
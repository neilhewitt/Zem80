using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RLC_RRC : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void RLC_r(RegisterName register)
        {
            byte value = RandomByte(0xE0);
            byte expected = (byte)(value << 1);
            Registers[register] = value;
            expected = expected.SetBit(0, value.GetBit(7));

            ExecutionResult executionResult = ExecuteInstruction($"RLC { register }");

            Flags flags = GetFlags(value, expected);

            Assert.That(
                Registers[register] == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void RRC_r(RegisterName register)
        {
            byte value = RandomByte(0xE0);
            byte expected = (byte)(value >> 1);
            Registers[register] = value;
            expected = expected.SetBit(7, value.GetBit(0));

            ExecutionResult executionResult = ExecuteInstruction($"RRC { register }");

            Flags flags = GetFlags(value, expected);

            Assert.That(
                Registers[register] == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test]
        public void RLC_xHL()
        {
            byte value = RandomByte(0xE0);
            byte expected = (byte)(value << 1);
            ushort address = RandomWord();
            WriteByteAt(address, value);
            Registers.HL = address;
            expected = expected.SetBit(0, value.GetBit(7));

            ExecutionResult executionResult = ExecuteInstruction($"RLC (HL)");

            Flags flags = GetFlags(value, expected);

            Assert.That(
                ReadByteAt(address) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test]
        public void RRC_xHL()
        {
            byte value = RandomByte(0xE0);
            byte expected = (byte)(value >> 1);
            ushort address = RandomWord();
            WriteByteAt(address, value);
            Registers.HL = address;
            expected = expected.SetBit(7, value.GetBit(0));

            ExecutionResult executionResult = ExecuteInstruction($"RRC (HL)");

            Flags flags = GetFlags(value, expected);

            Assert.That(
                ReadByteAt(address) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void RLC_xIndexOffset(RegisterPairName indexRegister)
        {
            byte value = RandomByte(0xE0);
            byte expected = (byte)(value << 1);
            ushort address = RandomWord();
            sbyte offset = (sbyte)RandomByte();
            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;
            expected = expected.SetBit(0, value.GetBit(7));

            ExecutionResult executionResult = ExecuteInstruction($"RLC ({ indexRegister }+o)", arg1: (byte)offset);

            Flags flags = GetFlags(value, expected);

            Assert.That(
                ReadByteAt((ushort)(address + offset)) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void RRC_xIndexOffset(RegisterPairName indexRegister)
        {
            byte value = RandomByte(0xE0);
            byte expected = (byte)(value >> 1);
            ushort address = RandomWord();
            sbyte offset = (sbyte)RandomByte();
            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;
            expected = expected.SetBit(7, value.GetBit(0));

            ExecutionResult executionResult = ExecuteInstruction($"RRC ({ indexRegister }+o)", arg1: (byte)offset);

            Flags flags = GetFlags(value, expected);

            Assert.That(
                ReadByteAt((ushort)(address + offset)) == expected &&
                CompareWithCPUFlags(flags)
                );
        }

        private Flags GetFlags(byte original, byte expected)
        {
            Flags flags = new Flags();
            flags.Carry = original.GetBit(7);
            if (((sbyte)expected) < 0) flags.Sign = true;
            if (expected == 0) flags.Zero = true;
            if (expected.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
            return flags;
        }
    }
}
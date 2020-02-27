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
        private Flags GetExpectedFlags(byte original, byte expected, bool carry)
        {
            Flags flags = new Flags();
            flags.Carry = carry;
            if (((sbyte)expected) < 0) flags.Sign = true;
            if (expected == 0) flags.Zero = true;
            if (expected.CountBits(true) % 2 == 0) flags.ParityOverflow = true;
            return flags;
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisters")]
        public void RLC_r(RegisterName register)
        {
            byte value = 0x7F;
            byte expected = (byte)(value << 1);
            Registers[register] = value;
            expected = expected.SetBit(0, value.GetBit(7));

            ExecutionResult executionResult = ExecuteInstruction($"RLC { register }");

            Flags expectedFlags = GetExpectedFlags(value, expected, expected.GetBit(0));

            Assert.That(Registers[register], Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisters")]
        public void RRC_r(RegisterName register)
        {
            byte value = 0x7F;
            byte expected = (byte)(value >> 1);
            Registers[register] = value;
            expected = expected.SetBit(7, value.GetBit(0));

            ExecutionResult executionResult = ExecuteInstruction($"RRC { register }");

            Flags expectedFlags =  GetExpectedFlags(value, expected, expected.GetBit(7));

            Assert.That(Registers[register], Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void RLC_xHL()
        {
            byte value = 0x7F;
            byte expected = (byte)(value << 1);
            ushort address = 0x5000;
            WriteByteAt(address, value);
            Registers.HL = address;
            expected = expected.SetBit(0, value.GetBit(7));

            ExecutionResult executionResult = ExecuteInstruction($"RLC (HL)");

            Flags expectedFlags =  GetExpectedFlags(value, expected, expected.GetBit(0));

            Assert.That(ReadByteAt(address), Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void RRC_xHL()
        {
            byte value = 0x7F;
            byte expected = (byte)(value >> 1);
            ushort address = 0x5000;
            WriteByteAt(address, value);
            Registers.HL = address;
            expected = expected.SetBit(7, value.GetBit(0));

            ExecutionResult executionResult = ExecuteInstruction($"RRC (HL)");

            Flags expectedFlags =  GetExpectedFlags(value, expected, expected.GetBit(7));

            Assert.That(ReadByteAt(address), Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void RLC_xIndexOffset([Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName indexRegister, [Values(127, -128)] sbyte offset)
        {
            byte value = 0x7F;
            byte expected = (byte)(value << 1);
            ushort address = 0x5000;
            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;
            expected = expected.SetBit(0, value.GetBit(7));

            ExecutionResult executionResult = ExecuteInstruction($"RLC ({ indexRegister }+o)", arg1: (byte)offset);

            Flags expectedFlags =  GetExpectedFlags(value, expected, expected.GetBit(0));

            Assert.That(ReadByteAtIndexAndOffset(indexRegister, offset), Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void RRC_xIndexOffset([Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName indexRegister, [Values(127, -128)] sbyte offset)
        {
            byte value = 0x7F;
            byte expected = (byte)(value >> 1);
            ushort address = 0x5000;
            WriteByteAt((ushort)(address + offset), value);
            Registers[indexRegister] = address;
            expected = expected.SetBit(7, value.GetBit(0));

            ExecutionResult executionResult = ExecuteInstruction($"RRC ({ indexRegister }+o)", arg1: (byte)offset);

            Flags expectedFlags =  GetExpectedFlags(value, expected, expected.GetBit(7));

            Assert.That(ReadByteAtIndexAndOffset(indexRegister, offset), Is.EqualTo(expected));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(expectedFlags));
        }
    }
}
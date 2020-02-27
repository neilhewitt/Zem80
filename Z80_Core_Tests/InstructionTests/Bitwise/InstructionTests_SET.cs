using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_SET : InstructionTestBase
    {
        [Test]
        public void SET_r()
        {
            Flags initialFlags = Flags;
            byte initialValue = 0x00;
            byte bitIndex = 6;
            byte setValue = initialValue.SetBit(bitIndex, true);

            Registers.B = initialValue;
            ExecutionResult executionResult = ExecuteInstruction($"SET { bitIndex },B", bitIndex: bitIndex);

            Assert.That(Registers.B, Is.EqualTo(setValue));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(initialFlags)); // flags should be preserved
        }

        [Test]
        public void SET_xHL()
        {
            Flags initialFlags = Flags;
            byte initialValue = 0x00;
            byte bitIndex = 5;
            byte setValue = initialValue.SetBit(bitIndex, true);
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, initialValue);

            ExecutionResult executionResult = ExecuteInstruction($"SET { bitIndex },(HL)", bitIndex: (byte)bitIndex);

            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(setValue));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(initialFlags));
        }

        [Test]
        public void SET_xIndexOffset([Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair, [Values(0x7F, -0x80)] sbyte offset)
        {
            Flags initialFlags = Flags;
            byte initialValue = 0x00;
            byte bitIndex = 4;
            byte setValue = initialValue.SetBit(bitIndex, true);
            ushort address = 0x5000;

            Registers[registerPair] = address;
            WriteByteAt((ushort)(address + offset), initialValue);

            ExecutionResult executionResult = ExecuteInstruction($"SET { bitIndex },({ registerPair }+o)", bitIndex: (byte)bitIndex, arg1: (byte)offset);

            Assert.That(ReadByteAtIndexAndOffset(registerPair, offset), Is.EqualTo(setValue));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(initialFlags));
        }
    }
}
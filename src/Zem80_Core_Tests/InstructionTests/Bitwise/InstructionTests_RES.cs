using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_RES : InstructionTestBase
    {
        [Test]
        public void RES_r()
        {
            Flags initialFlags = Flags;
            byte initialValue = 0xFF;
            byte bitIndex = 6;
            byte resetValue = initialValue.SetBit(bitIndex, false);
            
            Registers.B = initialValue;
            ExecutionResult executionResult = ExecuteInstruction($"RES { bitIndex },B");

            Assert.That(Registers.B, Is.EqualTo(resetValue));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(initialFlags)); // flags should be preserved
        }

        [Test]
        public void RES_xHL()
        {
            Flags initialFlags = Flags;
            byte initialValue = 0xFF;
            byte bitIndex = 5;
            byte resetValue = initialValue.SetBit(bitIndex, false);
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, initialValue);

            ExecutionResult executionResult = ExecuteInstruction($"RES { bitIndex },(HL)");

            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(resetValue));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(initialFlags)); 
        }

        [Test]
        public void RES_xIndexOffset([Values(WordRegister.IX, WordRegister.IY)] WordRegister registerPair, [Values(0x7F, -0x80)] sbyte offset)
        {
            Flags initialFlags = Flags;
            byte initialValue = 0xFF;
            byte bitIndex = 4;
            byte resetValue = initialValue.SetBit(bitIndex, false);
            ushort address = 0x5000;

            Registers[registerPair] = address;
            WriteByteAt((ushort)(address + offset), initialValue);

            ExecutionResult executionResult = ExecuteInstruction($"RES { bitIndex },({ registerPair }+o)", arg1:(byte)offset);

            Assert.That(ReadByteAtIndexAndOffset(registerPair, offset), Is.EqualTo(resetValue));
            Assert.That(CPU.Registers.Flags, Is.EqualTo(initialFlags));
        }
    }
}
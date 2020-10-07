using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_BIT : InstructionTestBase
    {
        [Test]
        public void BIT_b_r([Values(0, 1, 2, 3, 4, 5, 7)] ByteRegister register, [Range(0, 6)] int bitIndex)
        {
            byte value = 0x7F;
            bool zeroFlagExpected = value.GetBit(bitIndex) == false;

            Registers[register] = value;
            ExecutionResult executionResult = ExecuteInstruction($"BIT {bitIndex},{register}");

            Assert.That(
                executionResult.Flags.Check(
                    zero: zeroFlagExpected,
                    halfCarry: true
                    ),
                Is.True);
        }

        [Test]
        public void BIT_b_xHL([Range(0, 6)] int bitIndex)
        {
            byte value = 0x7F;
            ushort address = 0x5000;
            bool zeroFlagExpected = value.GetBit(bitIndex) == false;

            WriteByteAt(address, value);
            Registers.HL = address;
            ExecutionResult executionResult = ExecuteInstruction($"BIT {bitIndex},(HL)");

            Assert.That(
                executionResult.Flags.Check(
                    zero: zeroFlagExpected,
                    halfCarry: true
                    ),
                Is.True);
        }

        [Test]
        public void BIT_b_xIndexOffset([Values(WordRegister.IX, WordRegister.IY)] WordRegister registerPair, [Range(0, 6)] int bitIndex, [Values(0, 127, -127)] sbyte offset)
        {
            byte value = 0x7F;
            ushort address = 0x5000;
            bool zeroFlagExpected = value.GetBit(bitIndex) == false;

            WriteByteAt((ushort)(address + offset), value);
            Registers[registerPair] = address;
            ExecutionResult executionResult = ExecuteInstruction($"BIT {bitIndex},(HL)", arg1:(byte)offset);

            Assert.That(
                executionResult.Flags.Check(
                    zero: zeroFlagExpected,
                    halfCarry: true
                    ),
                Is.True);
        }
    }
}

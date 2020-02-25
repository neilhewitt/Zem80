using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_DEC : InstructionTestBase
    {
        private (byte, Flags) GetExpectedResultAndFlags(byte input, bool carry)
        {
            byte result = (byte)(input - 1);

            Flags flags = new Flags();
            flags.Carry = carry;
            flags.Zero = (result == 0);
            flags.Sign = ((sbyte)result < 0);
            flags.HalfCarry = ((result & 0x0F) == 0);
            flags.ParityOverflow = (input == 0x80);
            flags.Subtract = true;

            return (result, flags);
        }

        [Test]
        public void DEC_r([Values(0x7F, 0xFF, 0x01, 0x00)] byte input, [Values(true, false)] bool carry)
        {
            Registers.A = input;
            Registers.Flags.Carry = carry;

            ExecutionResult executionResult = ExecuteInstruction($"DEC A");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void DEC_rr([Values(0x7F00, 0xFFFF, 0x0001, 0x0000)] int input, [Values(true, false)] bool carry)
        {
            Registers.BC = (ushort)input;
            Registers.Flags.Carry = carry;

            ExecutionResult executionResult = ExecuteInstruction($"DEC BC");
            ushort expectedResult = (ushort)(input - 1);

            Assert.That(Registers.BC, Is.EqualTo(expectedResult)); // no flags affected by DEC rr
        }

        [Test]
        public void DEC_xHL([Values(0x7F, 0xFF, 0x01, 0x00)] byte input, [Values(true, false)] bool carry)
        {
            ushort address = 0x5000;
            Registers.HL = address;
            Registers.Flags.Carry = carry;
            WriteByteAt(address, input);

            ExecutionResult executionResult = ExecuteInstruction($"DEC (HL)");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, carry);

            Assert.That(ReadByteAt(address), Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void DEC_xIndexOffset([Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair, [Values(127, -127)] sbyte offset, 
            [Values(0x7F, 0xFF, 0x01, 0x00)] byte input, [Values(true, false)] bool carry)
        {
            ushort address = 0x5000;
            Registers[registerPair] = address;
            Registers.Flags.Carry = carry;
            WriteByteAtIndexAndOffset(registerPair, offset, input); // write input to address pointed to by (<index register> + offset)

            ExecutionResult executionResult = ExecuteInstruction($"DEC ({registerPair}+o)", arg1: (byte)offset);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, carry);

            Assert.That(ReadByteAtIndexAndOffset(registerPair, offset), Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }
    }
}
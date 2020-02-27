using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_XOR : InstructionTestBase
    {
        private (byte expectedResult, Flags expectedFlags) GetExpectedResultAndFlags(byte first, byte second)
        {
            short result = (short)(first ^ second);
            Flags flags = new Flags();

            flags.Zero = (result == 0);
            flags.Sign = ((sbyte)result < 0);
            flags.ParityOverflow = (result > 0xFF);

            return ((byte)result, flags);
        }

        private bool CheckFlags(Flags expected, Flags current)
        {
            return current.Check(
                    zero: expected.Zero,
                    sign: expected.Sign,
                    halfCarry: false,
                    parityOverflow: expected.ParityOverflow,
                    carry: false,
                    subtract: false
                );
        }

        [Test]
        public void XOR_r()
        {
            byte first = 0x7F;
            byte second = 0x33;

            Registers.A = first;
            Registers.B = second;
            ExecutionResult executionResult = ExecuteInstruction($"XOR B"); // if one register works, they all do
            (short expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(first, second);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(CheckFlags(expectedFlags, executionResult.Flags), Is.True); ;
        }

        [Test]
        public void XOR_n()
        {
            byte first = 0x7F;
            byte second = 0x33;

            Registers.A = first;
            ExecutionResult executionResult = ExecuteInstruction($"XOR n", arg1: second);
            (short expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(first, second);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(CheckFlags(expectedFlags, executionResult.Flags), Is.True); ;
        }

        [Test]
        public void XOR_xHL()
        {
            byte first = 0x7F;
            byte second = 0x33;

            Registers.A = first;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, second);

            ExecutionResult executionResult = ExecuteInstruction($"XOR (HL)"); 
            (short expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(first, second);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(CheckFlags(expectedFlags, executionResult.Flags), Is.True); ;
        }

        [Test]
        public void XOR_xIndexOffset([Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair, [Values(127, -128)] sbyte offset)
        {
            byte first = 0x7F;
            byte second = 0x33;

            Registers.A = first;
            Registers[registerPair] = 0x5000;
            WriteByteAtIndexAndOffset(registerPair, offset, second);

            ExecutionResult executionResult = ExecuteInstruction($"XOR ({ registerPair }+o)", arg1: (byte)offset);
            (short expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(first, second);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(CheckFlags(expectedFlags, executionResult.Flags), Is.True); ;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;
using NUnit.Framework;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_NEG : InstructionTestBase
    {
        [Test]
        public void NEG()
        {
            byte value = 0x7F;
            Registers.A = value;
            ExecutionResult executionResult = ExecuteInstruction("NEG");

            sbyte actual = (sbyte)Registers.A;
            sbyte expected = (sbyte)(0 - value);
            bool zero = expected == 0;
            bool sign = expected < 0;
            bool halfCarry = expected.HalfCarryWhenConvertingToByte();
            bool parityOverflow = (byte)expected == 0x80;
            bool carry = (byte)expected != 0x00;

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(executionResult.Flags.Check(
                    zero: zero, 
                    sign: sign, 
                    halfCarry: halfCarry, 
                    parityOverflow: parityOverflow, 
                    carry: carry
                ), Is.True);
        }
    }
}
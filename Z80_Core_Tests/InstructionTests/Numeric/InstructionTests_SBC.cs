using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_SBC : InstructionTestBase
    {
        private (byte, Flags) GetExpectedResultAndFlags(byte value, byte subtract, bool carry)
        {
            Flags flags = new Flags();
            if (carry) subtract--;

            ushort result = (ushort)(value - subtract);
            short signedResult = (short)result;

            flags.Carry = result > 0xFF;
            flags.ParityOverflow = (signedResult > 0x7F || signedResult < -0x80);
            flags.Zero = result == 0;
            flags.Sign = ((sbyte)signedResult < 0);
            flags.HalfCarry = value.HalfCarryWhenAdding(subtract);

            return ((byte)result, flags);
        }

        private (ushort, Flags) GetExpectedResultAndFlags(ushort value, ushort subtract, bool carry)
        {
            Flags flags = new Flags();
            if (carry) subtract--;

            uint result = (uint)(value - subtract);
            int signedResult = (int)result;

            flags.Carry = result > 0xFFFF;
            flags.ParityOverflow = (signedResult > 0x7FFF || signedResult < -0x8000);
            flags.Zero = result == 0;
            flags.Sign = ((short)signedResult < 0);
            flags.HalfCarry = value.HalfCarryWhenAdding(subtract);

            return ((ushort)result, flags);
        }

        [Test]
        public void SBC_A_r([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte subtract = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input; 
            Registers.B = subtract;

            ExecutionResult executionResult = ExecuteInstruction($"SBC A,B");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, subtract, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SBC_A_n([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte subtract = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"SBC A,n", arg1: subtract);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, subtract, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SBC_A_xHL([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry)
        {
            byte subtract = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, subtract);

            ExecutionResult executionResult = ExecuteInstruction($"SBC A,(HL)");
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, subtract, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SBC_A_xIndexOffset([Values(0x00, 0x7E, 0x7F, 0xFF)] byte input, [Values(true, false)] bool carry, [Values(127,-127)] sbyte offset, 
            [Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair)
        {
            byte subtract = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.A = input;
            Registers[registerPair] = 0x5000;
            WriteByteAt((ushort)(Registers[registerPair] + offset), subtract);

            ExecutionResult executionResult = ExecuteInstruction($"SBC A,({ registerPair }+o)", arg1: (byte)offset);
            (byte expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags(input, subtract, carry);

            Assert.That(Registers.A, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void SBC_HL_rr([Values(0x0000, 0x7FFE, 0x7FFF, 0xFFFF)] int input, [Values(true, false)] bool carry,
            [Values(RegisterPairName.BC, RegisterPairName.DE, RegisterPairName.SP)] RegisterPairName registerPair)
        {
            ushort subtract = 0x01; // with input range, covers all cases

            Flags.Carry = carry; // simulates previous Carry flag value
            Registers.HL = (ushort)input;
            Registers[registerPair] = subtract;

            ExecutionResult executionResult = ExecuteInstruction($"SBC HL,{ registerPair }");
            (ushort expectedResult, Flags expectedFlags) = GetExpectedResultAndFlags((ushort)input, subtract, carry);

            Assert.That(Registers.HL, Is.EqualTo(expectedResult));
            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }
    }
}

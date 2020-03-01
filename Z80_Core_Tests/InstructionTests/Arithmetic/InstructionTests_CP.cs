using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Z80.Core;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_CP : InstructionTestBase
    {
        private Flags GetExpectedFlags(byte input, byte compare)
        {
            Flags flags = new Flags();
            int result = input - compare;
            FlagHelper.SetFlagsFromArithmeticOperation(flags, input, compare, result, true);

            return flags;
        }

        [Test]
        public void CP_r([Values(0, 1, 2, 3, 4, 5, 7)] RegisterName register, [Values(0x7F, 0x80, 0xFF)] byte input)
        {
            Registers.A = input;
            if (register != RegisterName.A) Registers[register] = 0x80;

            ExecutionResult executionResult = ExecuteInstruction($"CP {register}");
            Flags expectedFlags = GetExpectedFlags(Registers.A, Registers[register]);

            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void CP_n([Values(0x7F, 0x80, 0xFF)] byte input)
        {
            Registers.A = input;

            ExecutionResult executionResult = ExecuteInstruction($"CP n", arg1: 0x80);
            Flags expectedFlags = GetExpectedFlags(Registers.A, 0x80);

            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void CP_xHL([Values(0x7F, 0x80, 0xFF)] byte input)
        {
            Registers.A = input;
            Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, 0x80);

            ExecutionResult executionResult = ExecuteInstruction($"CP (HL)");
            Flags expectedFlags = GetExpectedFlags(Registers.A, 0x80);

            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }

        [Test]
        public void CP_xIndexOffset([Values(RegisterPairName.IX, RegisterPairName.IY)] RegisterPairName registerPair, 
            [Values(0x7F, 0x80, 0xFF)] byte input, [Values(127, -127)] sbyte offset)
        {
            Registers.A = input;
            Registers[registerPair] = 0x5000;
            WriteByteAt((ushort)(Registers[registerPair] + offset), 0x80);

            ExecutionResult executionResult = ExecuteInstruction($"CP ({ registerPair }+o)", arg1: (byte)offset);
            Flags expectedFlags = GetExpectedFlags(Registers.A, 0x80);

            Assert.That(executionResult.Flags, Is.EqualTo(expectedFlags));
        }
    }
}

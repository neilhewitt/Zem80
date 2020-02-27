using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z80.Core;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_LD : InstructionTestBase
    {
        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisterPairings")]
        [TestCase(RegisterName.A, RegisterName.I)]
        [TestCase(RegisterName.A, RegisterName.R)]
        [TestCase(RegisterName.I, RegisterName.A)]
        [TestCase(RegisterName.R, RegisterName.A)]
        public void LD_r_r(RegisterName register1, RegisterName register2)
        {
            Registers[register2] = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD {register1},{register2}");
            Assert.That(Registers[register1], Is.EqualTo(Registers[register2]));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisters")]
        public void LD_r_n(RegisterName register)
        {
            byte value = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD {register},n", arg1: value);

            Assert.That(Registers[register] == value);
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisters")]
        public void LD_xHL_r(RegisterName register)
        {
            Registers.HL = 0x5000;
            Registers[register] = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD (HL),{register}");
            
            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(Registers[register]));
        }

        [Test]
        public void LD_xHL_n()
        {
            byte value = 0x7F;
            Registers.HL = 0x5000;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD (HL),n", arg1: value);
            
            Assert.That(ReadByteAt(Registers.HL), Is.EqualTo(value));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisters")]
        public void LD_r_xHL(RegisterName register)
        {
            ushort address = Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, 0x7F);
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD {register},(HL)");
            
            Assert.That(Registers[register], Is.EqualTo(ReadByteAt(address)));
        }

        [TestCase(RegisterPairName.BC)]
        [TestCase(RegisterPairName.DE)]
        public void LD_A_xrr(RegisterPairName registerPair)
        {
            Registers[registerPair] = 0x5000;
            WriteByteAt(Registers[registerPair], 0x7F);
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD A,({registerPair})");

            Assert.That(Registers.A, Is.EqualTo(ReadByteAt(Registers[registerPair])));
        }

        [Test]
        public void LD_A_xnn()
        {
            ushort address = 0x5000;
            WriteByteAt(address, 0x7F);
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD A,(nn)", arg1:address.LowByte(), arg2:address.HighByte());

            Assert.That(Registers.A, Is.EqualTo(ReadByteAt(address)));
        }

        [TestCase(RegisterPairName.BC)]
        [TestCase(RegisterPairName.DE)]
        public void LD_xrr_A(RegisterPairName registerPair)
        {
            Registers[registerPair] = 0x5000;
            Registers.A = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD ({registerPair}),A");

            Assert.That(ReadByteAt(Registers[registerPair]), Is.EqualTo(Registers.A));
        }

        [Test]
        public void LD_xnn_A()
        {
            ushort address = RandomWord(0xFFFE);
            Registers.A = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD (nn),A", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(ReadByteAt(address), Is.EqualTo(Registers.A));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisterAndIndexPairings")]
        public void LD_r_xIndexOffset(RegisterName register, RegisterPairName indexRegister)
        {
            sbyte offset = (sbyte)0x7F;
            Registers[indexRegister] = 0x5000;
            WriteByteAt(Registers[indexRegister], 0x7F);
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD ({indexRegister}+o),{register}", arg1: (byte)offset);
            
            Assert.That(Registers[register], Is.EqualTo(ReadByteAtIndexAndOffset(indexRegister, offset)));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisterAndIndexPairings")]
        public void LD_xIndexOffset_r(RegisterName register, RegisterPairName indexRegister)
        {
            sbyte offset = (sbyte)0x7F;
            Registers[indexRegister] = 0x5000;
            Registers[register] = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD ({indexRegister}+o),{register}", arg1: (byte)offset);
            
            Assert.That(ReadByteAtIndexAndOffset(indexRegister, offset), Is.EqualTo(Registers[register]));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetIndexRegisters")]
        public void LD_xIndexOffset_n(RegisterPairName indexRegister)
        {
            byte value = 0x7F;
            sbyte offset = (sbyte)0x7F;
            Registers[indexRegister] = 0x5000;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD ({indexRegister}+o),n", arg1: (byte)offset, arg2: value);
            
            Assert.That(ReadByteAtIndexAndOffset(indexRegister, offset), Is.EqualTo(value));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisterPairs")]
        public void LD_rr_nn(RegisterPairName registerPair)
        {
            ushort value = 0x5000;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD {registerPair},nn", arg1:value.LowByte(), arg2:value.HighByte());

            Assert.That(Registers[registerPair], Is.EqualTo(value));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisterPairs")]
        public void LD_rr_xnn(RegisterPairName registerPair)
        {
            ushort address = RandomWord(0xFFFE);
            ushort value = 0x5000;
            WriteWordAt(address, value);
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD {registerPair},(nn)", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(Registers[registerPair], Is.EqualTo(ReadWordAt(address)));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisterPairs")]
        public void LD_xnn_rr(RegisterPairName registerPair)
        {
            ushort address = RandomWord(0xFFFE);
            Registers[registerPair] = 0x5000;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD (nn),{registerPair}", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(ReadWordAt(address), Is.EqualTo(Registers[registerPair]));
        }

        [TestCase(RegisterPairName.HL)]
        [TestCase(RegisterPairName.IX)]
        [TestCase(RegisterPairName.IY)]
        public void LD_SP_rr(RegisterPairName registerPair)
        {
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD SP,{registerPair}");
            Assert.That(Registers.SP, Is.EqualTo(Registers[registerPair]));
        }
    }
}

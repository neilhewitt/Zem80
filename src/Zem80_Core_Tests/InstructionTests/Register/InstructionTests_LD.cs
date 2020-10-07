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
        [TestCase(ByteRegister.A, ByteRegister.I)]
        [TestCase(ByteRegister.A, ByteRegister.R)]
        [TestCase(ByteRegister.I, ByteRegister.A)]
        [TestCase(ByteRegister.R, ByteRegister.A)]
        public void LD_r_r(ByteRegister register1, ByteRegister register2)
        {
            Registers[register2] = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD {register1},{register2}");
            Assert.That(Registers[register1], Is.EqualTo(Registers[register2]));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetByteRegisters")]
        public void LD_r_n(ByteRegister register)
        {
            byte value = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD {register},n", arg1: value);

            Assert.That(Registers[register] == value);
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetByteRegisters")]
        public void LD_xHL_r(ByteRegister register)
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

        [Test, TestCaseSource(typeof(LD_TestCases), "GetByteRegisters")]
        public void LD_r_xHL(ByteRegister register)
        {
            ushort address = Registers.HL = 0x5000;
            WriteByteAt(Registers.HL, 0x7F);
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD {register},(HL)");
            
            Assert.That(Registers[register], Is.EqualTo(ReadByteAt(address)));
        }

        [TestCase(WordRegister.BC)]
        [TestCase(WordRegister.DE)]
        public void LD_A_xrr(WordRegister registerPair)
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

        [TestCase(WordRegister.BC)]
        [TestCase(WordRegister.DE)]
        public void LD_xrr_A(WordRegister registerPair)
        {
            Registers[registerPair] = 0x5000;
            Registers.A = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD ({registerPair}),A");

            Assert.That(ReadByteAt(Registers[registerPair]), Is.EqualTo(Registers.A));
        }

        [Test]
        public void LD_xnn_A()
        {
            ushort address = 0x5000;
            Registers.A = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD (nn),A", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(ReadByteAt(address), Is.EqualTo(Registers.A));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisterAndIndexPairings")]
        public void LD_r_xIndexOffset(ByteRegister register, WordRegister indexRegister)
        {
            sbyte offset = (sbyte)0x7F;
            Registers[indexRegister] = 0x5000;
            WriteByteAt(Registers[indexRegister], 0x7F);
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD ({indexRegister}+o),{register}", arg1: (byte)offset);
            
            Assert.That(Registers[register], Is.EqualTo(ReadByteAtIndexAndOffset(indexRegister, offset)));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetRegisterAndIndexPairings")]
        public void LD_xIndexOffset_r(ByteRegister register, WordRegister indexRegister)
        {
            sbyte offset = (sbyte)0x7F;
            Registers[indexRegister] = 0x5000;
            Registers[register] = 0x7F;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD ({indexRegister}+o),{register}", arg1: (byte)offset);
            
            Assert.That(ReadByteAtIndexAndOffset(indexRegister, offset), Is.EqualTo(Registers[register]));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetIndexRegisters")]
        public void LD_xIndexOffset_n(WordRegister indexRegister)
        {
            byte value = 0x7F;
            sbyte offset = (sbyte)0x7F;
            Registers[indexRegister] = 0x5000;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD ({indexRegister}+o),n", arg1: (byte)offset, arg2: value);
            
            Assert.That(ReadByteAtIndexAndOffset(indexRegister, offset), Is.EqualTo(value));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetWordRegisters")]
        public void LD_rr_nn(WordRegister registerPair)
        {
            ushort value = 0x5000;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD {registerPair},nn", arg1:value.LowByte(), arg2:value.HighByte());

            Assert.That(Registers[registerPair], Is.EqualTo(value));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetWordRegisters")]
        public void LD_rr_xnn(WordRegister registerPair)
        {
            ushort address = 0x2000;
            ushort value = 0x5000;
            WriteWordAt(address, value);
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD {registerPair},(nn)", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(Registers[registerPair], Is.EqualTo(ReadWordAt(address)));
        }

        [Test, TestCaseSource(typeof(LD_TestCases), "GetWordRegisters")]
        public void LD_xnn_rr(WordRegister registerPair)
        {
            ushort address = 0x2000;
            Registers[registerPair] = 0x5000;
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD (nn),{registerPair}", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(ReadWordAt(address), Is.EqualTo(Registers[registerPair]));
        }

        [TestCase(WordRegister.HL)]
        [TestCase(WordRegister.IX)]
        [TestCase(WordRegister.IY)]
        public void LD_SP_rr(WordRegister registerPair)
        {
            ExecutionResult executionResult = ExecuteInstruction(mnemonic: $"LD SP,{registerPair}");
            Assert.That(Registers.SP, Is.EqualTo(Registers[registerPair]));
        }
    }
}

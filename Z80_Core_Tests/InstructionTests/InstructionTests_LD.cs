﻿using NUnit.Framework;
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
        [Test, TestCaseSource(typeof(TestCases), "GetRegisterPairings")]
        [TestCase(RegisterIndex.A, RegisterIndex.I)]
        [TestCase(RegisterIndex.A, RegisterIndex.R)]
        [TestCase(RegisterIndex.I, RegisterIndex.A)]
        [TestCase(RegisterIndex.R, RegisterIndex.A)]
        public void LD_r_r(RegisterIndex register1, RegisterIndex register2)
        {
            var result = Execute(mnemonic: $"LD {register1},{register2}", registerIndex: register2, bitIndex: (byte)register1);
            Assert.That(Registers[register1] == Registers[register2]);
        }

        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void LD_r_n(RegisterIndex register)
        {
            byte value = RandomByte();
            var result = Execute(mnemonic: $"LD {register},n", arg1: value, registerIndex: register);

            Assert.That(Registers[register] == value);
        }

        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void LD_xHL_r(RegisterIndex register)
        {
            var result = Execute(mnemonic: $"LD (HL),{register}", registerIndex: register);
            Assert.That(ByteAt(Registers.HL) == Registers[register]);
        }

        [Test]
        public void LD_xHL_n()
        {
            byte value = RandomByte();
            var result = Execute(mnemonic: $"LD (HL),n", arg1: value);
            Assert.That(ByteAt(Registers.HL) == value);
        }

        [Test, TestCaseSource(typeof(TestCases), "GetRegisters")]
        public void LD_r_xHL(RegisterIndex register)
        {
            var result = Execute(mnemonic: $"LD {register},(HL)", registerIndex: register);

            Assert.That(Registers[register] == ByteAt(Registers.HL));
        }

        [TestCase(RegisterPairIndex.BC)]
        [TestCase(RegisterPairIndex.DE)]
        public void LD_A_xrr(RegisterPairIndex registerPair)
        {
            var result = Execute(mnemonic: $"LD A,({registerPair})");

            Assert.That(Registers.A == ByteAt(Registers[registerPair]));
        }

        [Test]
        public void LD_A_xnn()
        {
            ushort address = RandomWord(0xFFFE);
            var result = Execute(mnemonic: $"LD A,(nn)", arg1:address.HighByte(), arg2:address.LowByte());

            Assert.That(Registers.A == ByteAt(address));
        }

        [TestCase(RegisterPairIndex.BC)]
        [TestCase(RegisterPairIndex.DE)]
        public void LD_xrr_A(RegisterPairIndex registerPair)
        {
            var result = Execute(mnemonic: $"LD ({registerPair}),A");

            Assert.That(ByteAt(Registers[registerPair]) == Registers.A);
        }

        [Test]
        public void LD_xnn_A()
        {
            ushort address = RandomWord(0xFFFE);
            var result = Execute(mnemonic: $"LD (nn),A", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(ByteAt(address) == Registers.A);
        }

        [Test, TestCaseSource(typeof(TestCases), "GetRegisterAndIndexPairings")]
        public void LD_r_xIndexOffset(RegisterIndex register, RegisterPairIndex indexRegister)
        {
            byte offset = RandomByte(0x7F);
            var result = Execute(mnemonic: $"LD ({indexRegister}+o),{register}", arg1: offset, registerIndex: register);
            
            Assert.That(Registers[register] == ByteAt(indexRegister, offset));
        }

        [Test, TestCaseSource(typeof(TestCases), "GetRegisterAndIndexPairings")]
        public void LD_xIndexOffset_r(RegisterIndex register, RegisterPairIndex indexRegister)
        {
            byte offset = RandomByte(0x7F);
            var result = Execute(mnemonic: $"LD ({indexRegister}+o),{register}", arg1:offset, registerIndex: register);
            
            Assert.That(ByteAt(indexRegister, offset) == Registers[register]);
        }

        [Test, TestCaseSource(typeof(TestCases), "GetIndexRegisters")]
        public void LD_xIndexOffset_n(RegisterPairIndex indexRegister)
        {
            byte value = RandomByte();
            byte offset = RandomByte(0x7F);
            var result = Execute(mnemonic: $"LD ({indexRegister}+o),n", arg1: offset, arg2: value);
            
            Assert.That(ByteAt(indexRegister, offset) == value);
        }

        [Test, TestCaseSource(typeof(TestCases), "GetRegisterPairs")]
        public void LD_rr_nn(RegisterPairIndex registerPair)
        {
            ushort value = RandomWord();
            var result = Execute(mnemonic: $"LD {registerPair},nn", arg1:value.LowByte(), arg2:value.HighByte());

            Assert.That(Registers[registerPair] == value);
        }

        [Test, TestCaseSource(typeof(TestCases), "GetRegisterPairs")]
        public void LD_rr_xnn(RegisterPairIndex registerPair)
        {
            ushort address = RandomWord(0xFFFE);
            ushort value = RandomWord();
            _cpu.Memory.WriteWordAt(address, value);
            var result = Execute(mnemonic: $"LD {registerPair},(nn)", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(Registers[registerPair] == WordAt(address));
        }

        [Test, TestCaseSource(typeof(TestCases), "GetRegisterPairs")]
        public void LD_xnn_rr(RegisterPairIndex registerPair)
        {
            ushort address = RandomWord(0xFFFE);
            ushort value = RandomWord();
            Registers[registerPair] = value;
            var result = Execute(mnemonic: $"LD (nn),{registerPair}", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(WordAt(address) == value);
        }

        [TestCase(RegisterPairIndex.HL)]
        [TestCase(RegisterPairIndex.IX)]
        [TestCase(RegisterPairIndex.IY)]
        public void LD_SP_rr(RegisterPairIndex registerPair)
        {
            var result = Execute(mnemonic: $"LD SP,{registerPair}");
            Assert.That(Registers.SP == Registers[registerPair]);
        }
    }
}

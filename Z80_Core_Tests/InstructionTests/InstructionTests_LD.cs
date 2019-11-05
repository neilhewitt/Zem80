using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z80.Core;

namespace Z80.Core.Tests
{
    [TestFixture]
    public class InstructionTests_LD
    {
        private Processor _cpu;
        private Random _random;

        public IRegisters Registers => _cpu.Registers;

        #region setup

        [OneTimeSetUp]
        public void Setup()
        {
            _cpu = Bootstrapper.BuildDefault();
            _random = new Random(DateTime.Now.Millisecond);
        }

        [SetUp]
        public void Init()
        {
            _cpu.Reset(true); // clears RAM, registers etc between tests
        }

        #endregion

        #region helpers

        public ExecutionResult Execute(string mnemonic, byte? arg1 = null, byte? arg2 = null, byte? bitIndex = null, RegisterIndex? registerIndex = null)
        {
            Instruction instruction = Instruction.FindByMnemonic(mnemonic);
            InstructionData data = new InstructionData() {
                Opcode = instruction.Opcode,
                Argument1 = arg1 ?? 0,
                Argument2 = arg2 ?? 0,
                BitIndex = bitIndex ?? 0,
                RegisterIndex = registerIndex ?? RegisterIndex.None,
                DirectIX = instruction.Mnemonic.Contains("IX") && !instruction.Mnemonic.Contains("(IX)"),
                DirectIY = instruction.Mnemonic.Contains("IY") && !instruction.Mnemonic.Contains("(IY)"),
                IndexIX = instruction.Mnemonic.Contains("(IX)"),
                IndexIY = instruction.Mnemonic.Contains("(IY)")
            };
            return instruction.Implementation.Execute(_cpu, new InstructionPackage(instruction, data));
        }

        public byte RandomByte(byte maxValue = 0xFF)
        {
            return (byte)_random.Next(0x00, maxValue);
        }

        public ushort RandomWord(ushort maxValue = 0xFFFF)
        {
            return (ushort)_random.Next(0x00, maxValue);
        }

        public byte ByteAt(RegisterPairIndex indexRegister, byte offset)
        {
            return _cpu.Memory.ReadByteAt((ushort)(Registers[indexRegister] + offset));
        }

        public byte ByteAt(ushort address)
        {
            return _cpu.Memory.ReadByteAt(address);
        }

        #endregion

        [Test, TestCaseSource("GetRegisterPairings")]
        [TestCase(RegisterIndex.A, RegisterIndex.I)]
        [TestCase(RegisterIndex.A, RegisterIndex.R)]
        [TestCase(RegisterIndex.I, RegisterIndex.A)]
        [TestCase(RegisterIndex.R, RegisterIndex.A)]
        public void LD_r_r(RegisterIndex register1, RegisterIndex register2)
        {
            var result = Execute(mnemonic: $"LD {register1},{register2}", registerIndex: register2, bitIndex: (byte)register1);
            Assert.That(Registers[register1] == Registers[register2]);
        }

        [Test, TestCaseSource("GetRegisters")]
        public void LD_r_n(RegisterIndex register)
        {
            byte value = RandomByte();
            var result = Execute(mnemonic: $"LD {register},n", arg1: value, registerIndex: register);

            Assert.That(Registers[register] == value);
        }

        [Test, TestCaseSource("GetRegisters")]
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

        [Test, TestCaseSource("GetRegisters")]
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

        [Test, TestCaseSource("GetRegisterAndIndexPairings")]
        public void LD_r_xIndexOffset(RegisterIndex register, RegisterPairIndex indexRegister)
        {
            byte offset = RandomByte(0x7F);
            var result = Execute(mnemonic: $"LD ({indexRegister}+o),{register}", arg1: offset, registerIndex: register);
            
            Assert.That(Registers[register] == ByteAt(indexRegister, offset));
        }

        [Test, TestCaseSource("GetRegisterAndIndexPairings")]
        public void LD_xIndexOffset_r(RegisterIndex register, RegisterPairIndex indexRegister)
        {
            byte offset = RandomByte(0x7F);
            var result = Execute(mnemonic: $"LD ({indexRegister}+o),{register}", arg1:offset, registerIndex: register);
            
            Assert.That(ByteAt(indexRegister, offset) == Registers[register]);
        }

        [Test, TestCaseSource("GetIndexRegisters")]
        public void LD_xIndexOffset_n(RegisterPairIndex indexRegister)
        {
            byte value = RandomByte();
            byte offset = RandomByte(0x7F);
            var result = Execute(mnemonic: $"LD ({indexRegister}+o),n", arg1: offset, arg2: value);
            
            Assert.That(ByteAt(indexRegister, offset) == value);
        }

        [Test, TestCaseSource("GetRegisterPairs")]
        public void LD_rr_nn(RegisterPairIndex registerPair)
        {
            ushort value = RandomWord();
            var result = Execute(mnemonic: $"LD {registerPair},nn", arg1:value.LowByte(), arg2:value.HighByte());

            Assert.That(Registers[registerPair] == value);
        }

        [Test, TestCaseSource("GetRegisterPairs")]
        public void LD_rr_xnn(RegisterPairIndex registerPair)
        {
            ushort address = RandomWord(0xFFFE);
            ushort value = RandomWord();
            _cpu.Memory.WriteWordAt(address, value);
            var result = Execute(mnemonic: $"LD {registerPair},(nn)", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(Registers[registerPair] == _cpu.Memory.ReadWordAt(address));
        }

        [Test, TestCaseSource("GetRegisterPairs")]
        public void LD_xnn_rr(RegisterPairIndex registerPair)
        {
            ushort address = RandomWord(0xFFFE);
            ushort value = RandomWord();
            Registers[registerPair] = value;
            var result = Execute(mnemonic: $"LD (nn),{registerPair}", arg1: address.LowByte(), arg2: address.HighByte());

            Assert.That(_cpu.Memory.ReadWordAt(address) == value);
        }

        [TestCase(RegisterPairIndex.HL)]
        [TestCase(RegisterPairIndex.IX)]
        [TestCase(RegisterPairIndex.IY)]
        public void LD_SP_rr(RegisterPairIndex registerPair)
        {
            var result = Execute(mnemonic: $"LD SP,{registerPair}");
            Assert.That(Registers.SP == Registers[registerPair]);
        }


        #region test case sources

        private static IEnumerable<RegisterPairIndex> GetRegisterPairs()
        {
            return new List<RegisterPairIndex>()
            {
                RegisterPairIndex.BC,
                RegisterPairIndex.DE,
                RegisterPairIndex.HL,
                RegisterPairIndex.IX,
                RegisterPairIndex.IY,
                RegisterPairIndex.SP
            };
        }

        private static IEnumerable<RegisterIndex> GetRegisters()
        {
            IList<RegisterIndex> cases = new System.Collections.Generic.List<RegisterIndex>();
            for (int i = 0; i <= 7; i++)
            {
                RegisterIndex index = (RegisterIndex)i;
                if (i != 6)
                {
                    cases.Add(index);
                }
            }

            return cases;
        }

        private static IEnumerable<object[]> GetRegisterPairings()
        {
            IList<object[]> cases = new System.Collections.Generic.List<object[]>();
            for (int i = 0; i <= 7; i++)
            {
                RegisterIndex left = (RegisterIndex)i;
                for (int j = 0; j <= 7; j++)
                {
                    RegisterIndex right = (RegisterIndex)j;
                    if (i != 6 && j != 6)
                    {
                        cases.Add(new object[] { left, right });
                    }
                }
            }

            return cases;
        }

        private static IEnumerable<object[]> GetRegisterAndIndexPairings()
        {
            IList<object[]> cases = new System.Collections.Generic.List<object[]>();
            for (int i = 0; i <= 7; i++)
            {
                RegisterIndex index = (RegisterIndex)i;
                if (i != 6)
                {
                    cases.Add(new object[] { index, RegisterPairIndex.IX });
                    cases.Add(new object[] { index, RegisterPairIndex.IY });
                }
            }

            return cases;
        }

        private static IEnumerable<object[]> GetIndexRegisters()
        {
            IList<object[]> cases = new System.Collections.Generic.List<object[]>();
            cases.Add(new object[] { RegisterPairIndex.IX });
            cases.Add(new object[] { RegisterPairIndex.IY });

            return cases;
        }


        #endregion
    }
}

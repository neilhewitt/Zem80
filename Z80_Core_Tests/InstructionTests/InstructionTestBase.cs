using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    public abstract class InstructionTestBase
    {
        protected Processor _cpu;
        protected Random _random;

        public IRegisters Registers => _cpu.Registers;

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

        public ExecutionResult Execute(string mnemonic, byte? arg1 = null, byte? arg2 = null, byte? bitIndex = null, RegisterIndex? registerIndex = null)
        {
            Instruction instruction = Instruction.FindByMnemonic(mnemonic);
            InstructionData data = new InstructionData()
            {
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
            
            ExecutionResult result = instruction.Implementation.Execute(_cpu, new InstructionPackage(instruction, data));
            Registers.SetFlags(result.Flags);
            return result;
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

        public ushort WordAt(ushort address)
        {
            return _cpu.Memory.ReadWordAt(address);
        }

        public void WriteByteAt(ushort address, byte value)
        {
            _cpu.Memory.WriteByteAt(address, value);
        }

        public void WriteWordAt(ushort address, ushort value)
        {
            _cpu.Memory.WriteWordAt(address, value);
        }
    }
}

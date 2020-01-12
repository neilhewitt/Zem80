using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Z80.Core.Tests
{
    public abstract class InstructionTestBase
    {
        protected IDebugProcessor _cpu;
        protected Random _random;

        public IRegisters Registers => _cpu.Registers;
        public IFlags Flags => _cpu.Registers.Flags;

        [OneTimeSetUp]
        public void Setup()
        {
            _cpu = Bootstrapper.BuildDebugCPU();
            _random = new Random(DateTime.Now.Millisecond);
        }

        [SetUp]
        public void Init()
        {
            _cpu.ResetAndClearMemory();
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
            
            ExecutionResult result = _cpu.ExecuteDirect(instruction, data); // only available on ITestProcessor test/debug interface - sets flags but not PC
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

        public bool RandomBool()
        {
            return (RandomByte(1) == 0);
        }

        public byte ByteAt(RegisterPairIndex indexRegister, sbyte offset)
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

        public bool TestFlags(bool? sign = null, bool? carry = null, bool? halfCarry = null, bool? parityOverflow = null, bool? subtract = null, bool? zero = null)
        {
            IFlags f = Registers.Flags;
            return (sign.HasValue ? f.Sign == sign : true &&
                    carry.HasValue ? f.Carry == carry : true &&
                    halfCarry.HasValue ? f.HalfCarry == halfCarry : true &&
                    parityOverflow.HasValue ? f.ParityOverflow == parityOverflow : true &&
                    subtract.HasValue ? f.Subtract == subtract : true &&
                    zero.HasValue ? f.Zero == zero : true);
        }

        public void PresetFlags(bool? sign = null, bool? carry = null, bool? halfCarry = null, bool? parityOverflow = null, bool? subtract = null, bool? zero = null)
        {
            IFlags f = new Flags()
            {
                Sign = sign ?? false, Carry = carry ?? false, HalfCarry = halfCarry ?? false, ParityOverflow = parityOverflow ?? false, Subtract = subtract ?? false, Zero = zero ?? false 
            };

            _cpu.Registers.SetFlags(f);
        }
    }
}

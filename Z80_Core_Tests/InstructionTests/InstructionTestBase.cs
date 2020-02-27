using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z80.Core.Tests
{
    public abstract class InstructionTestBase
    {
        protected Random _random;

        public IDebugProcessor CPU { get; private set; }
        public IRegisters Registers => CPU.Registers;
        public Flags Flags => CPU.Registers.Flags;

        [OneTimeSetUp]
        public void Setup()
        {
            CPU = Bootstrapper.BuildCPU().Debuggable;
            _random = new Random(DateTime.Now.Millisecond);
        }

        [SetUp]
        public void Init()
        {
            CPU.ResetAndClearMemory();
        }

        public ExecutionResult ExecuteInstruction(string mnemonic, byte? arg1 = null, byte? arg2 = null, byte? bitIndex = null/*, RegisterName? register = null*/)
        {
            Instruction instruction = Instruction.FindByMnemonic(mnemonic);
            InstructionData data = new InstructionData()
            {
                Argument1 = arg1 ?? 0,
                Argument2 = arg2 ?? 0
            };
            
            ExecutionResult result = CPU.Execute(new InstructionPackage(instruction, data)); // only available on IDebugProcessor debug interface - sets flags but does not advance PC
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

        public RegisterName RandomRegister()
        {
            int random = _random.Next(0, 7);
            if (random == 6) return RandomRegister();
            return (RegisterName)random;
        }

        public byte ReadByteAtIndexAndOffset(RegisterPairName indexRegister, sbyte offset)
        {
            return CPU.Memory.ReadByteAt((ushort)(Registers[indexRegister] + offset));
        }

        public byte ReadByteAt(ushort address)
        {
            return CPU.Memory.ReadByteAt(address);
        }

        public ushort ReadWordAt(ushort address)
        {
            return CPU.Memory.ReadWordAt(address);
        }

        public void WriteByteAt(ushort address, byte value)
        {
            CPU.Memory.WriteByteAt(address, value);
        }

        public void WriteByteAtIndexAndOffset(RegisterPairName indexRegister, sbyte offset, byte value)
        {
            CPU.Memory.WriteByteAt((ushort)(Registers[indexRegister] + offset), value);
        }

        public void WriteWordAt(ushort address, ushort value)
        {
            CPU.Memory.WriteWordAt(address, value);
        }

        public bool CompareWithCPUFlags(bool? sign = null, bool? carry = null, bool? halfCarry = null, bool? parityOverflow = null, bool? subtract = null, bool? zero = null)
        {
            Flags f = Registers.Flags;
            return (sign.HasValue ? f.Sign == sign : true &&
                    carry.HasValue ? f.Carry == carry : true &&
                    halfCarry.HasValue ? f.HalfCarry == halfCarry : true &&
                    parityOverflow.HasValue ? f.ParityOverflow == parityOverflow : true &&
                    subtract.HasValue ? f.Subtract == subtract : true &&
                    zero.HasValue ? f.Zero == zero : true);
        }

        public bool CompareWithCPUFlags(Flags flags)
        {
            if (flags != null)
            {
                return flags.Value == CPU.Registers.Flags.Value;
            }

            return false;
        }

        public void PresetFlags(bool? sign = null, bool? carry = null, bool? halfCarry = null, bool? parityOverflow = null, bool? subtract = null, bool? zero = null)
        {
            Flags f = new Flags()
            {
                Sign = sign ?? false, Carry = carry ?? false, HalfCarry = halfCarry ?? false, ParityOverflow = parityOverflow ?? false, Subtract = subtract ?? false, Zero = zero ?? false 
            };

            CPU.Registers.Flags.Set(f.Value);
        }

        public byte[] RandomBytes(int size)
        {
            return Enumerable.Range(0, size).Select(x => RandomByte()).ToArray();
        }
    }


    public enum WriteDirection { Incrementing, Decrementing }
    public enum WriteRepeats { Repeating, NotRepeating }
}

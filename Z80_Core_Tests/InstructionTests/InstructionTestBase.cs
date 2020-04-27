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

        public Processor CPU { get; private set; }
        public Registers Registers => CPU.Registers;
        public Flags Flags => CPU.Registers.Flags;

        [OneTimeSetUp]
        public void Setup()
        {
            CPU = Bootstrapper.BuildCPU();
            _random = new Random(DateTime.Now.Millisecond);
        }

        [SetUp]
        public void Init()
        {
            CPU.ResetAndClearMemory();
        }

        public ExecutionResult ExecuteInstruction(string mnemonic, byte? arg1 = null, byte? arg2 = null)
        {
            Instruction instruction = InstructionSet.InstructionsByMnemonic[mnemonic];
            InstructionData data = new InstructionData()
            {
                Argument1 = arg1 ?? 0,
                Argument2 = arg2 ?? 0
            };
            
            ExecutionResult result = CPU.Debug.Execute(new ExecutionPackage(instruction, data)); // only available on Processor debug interface - sets flags but does not advance PC
            return result;
        }

        public byte ReadByteAtIndexAndOffset(WordRegister indexRegister, sbyte offset)
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

        public void WriteByteAtIndexAndOffset(WordRegister indexRegister, sbyte offset, byte value)
        {
            CPU.Memory.WriteByteAt((ushort)(Registers[indexRegister] + offset), value);
        }

        public void WriteWordAt(ushort address, ushort value)
        {
            CPU.Memory.WriteWordAt(address, value);
        }

        public bool RandomBool()
        {
            return _random.Next(0x00, 0x01) == 0x00;
        }

        public byte[] RandomBytes(int size)
        {
            return Enumerable.Range(0, size).Select(x => (byte)_random.Next(0x00, 0xFF)).ToArray();
        }
    }


    public enum WriteDirection { Incrementing, Decrementing }
    public enum WriteRepeats { Repeating, NotRepeating }
}

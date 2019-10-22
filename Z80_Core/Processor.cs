using System;
using System.Timers;

namespace Z80.Core
{
    public class Processor
    {
        private bool _running;
        private InstructionDecoder _decoder = new InstructionDecoder();

        public IRegisters Registers { get; private set; }
        public IFlags Flags { get; private set; }
        public IMemory Memory { get; private set; }
        public Stack Stack { get; private set; }
        public ushort AddressBus { get; private set; }
        public byte DataBus { get; private set; }
        public InterruptMode InterruptMode { get; private set; } = InterruptMode.Zero;
        public double SpeedInMhz { get; private set; }

        public void Start()
        {
            Registers.Clear();
            
            _running = true;

            while (_running)
            {
                InstructionCycle();
            }
        }

        public void Stop()
        {
            _running = false;
        }

        private void InstructionCycle()
        {
            try
            {
                byte[] instruction = Memory.ReadBytesAt(Registers.PC, 4); // will succeed even if PC overflows - overflow bytes come back as 0x00
                InstructionPackage package = _decoder.Decode(instruction, Registers.PC);
                ExecutionResult result = package.Instruction.Implementation.Execute(this, package);
                Registers.Flags.SetFrom(result.Flags);
                if (!result.PCWasSet) Registers.PC += package.Instruction.SizeInBytes;
            }
            catch
            {
                Stop();
                throw;
            }
        }

        internal Processor(IRegisters registers, IFlags flags, IMemory memory, ushort stackPointer, double speedInMHz)
        {
            Registers = registers;
            Flags = flags;
            Memory = memory;
            Registers.SP = stackPointer;
            Stack = new Stack(this, stackPointer);
            SpeedInMhz = speedInMHz;
        }
    }
}

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
            byte[] instruction = Memory.ReadBytesAt(Registers.PC, 4); // will succeed even if PC overflows - overflow bytes come back as 0x00
            InstructionPackage package = _decoder.Decode(instruction, Registers.PC);
            // TODO: actually do stuff...
           
            if (!Registers.AdvancePC(package.Instruction.SizeInBytes))
            {
                Stop();
                return;
            }

            // handle timing            
        }

        internal Processor(IRegisters registers, IFlags flags, IMemory memory, double speedInMHz)
        {
            Registers = registers;
            Flags = flags;
            Memory = memory;
            SpeedInMhz = speedInMHz;
        }
    }
}

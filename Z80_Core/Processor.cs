using System;

namespace Z80.Core
{
    public class Processor
    {
        private bool _running;

        public IRegisters Registers { get; private set; }
        public IMemory Memory { get; private set; }
        public ushort AddressBus { get; private set; }
        public byte DataBus { get; private set; }
        public InterruptMode InterruptMode { get; private set; } = InterruptMode.Zero;

        public event EventHandler OnBeforeFetch;
        public event EventHandler<byte[]> OnBeforeDecode;
        public event EventHandler<InstructionPackage> OnBeforeExecute;
        public event EventHandler<InstructionPackage> OnAfterExecute;

        public void Start()
        {
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
            // skeleton events for instruction cycle
            OnBeforeFetch(this, EventArgs.Empty);
            byte[] instruction = Fetch();

            OnBeforeDecode(this, instruction);
            InstructionPackage package = Decode(instruction);

            OnBeforeExecute(this, package);
            // TODO: actually do stuff...
            OnAfterExecute(this, package);

            if (Registers.PC + package.Info.SizeInBytes > (Memory.SizeInKilobytes * 1024)) // overflow!
            {
                Stop();
            }
            else
            {
                Registers.PC += package.Info.SizeInBytes; // move the program counter on
            }

            // handle timing
        }

        private byte[] Fetch()
        {
            return Memory.ReadBytesAt(Registers.PC, 4);
        }

        private InstructionPackage Decode(byte[] instruction)
        {
            InstructionDecoder decoder = new InstructionDecoder();
            InstructionInfo info = decoder.Decode(instruction, Registers.PC, out InstructionData data);
            return new InstructionPackage(info, data);
        }

        internal Processor(IRegisters registers, IMemory memory)
        {
            Registers = registers;
            Memory = memory;

            // event sinks
            OnBeforeFetch += (x, y) => { };
            OnBeforeDecode += (x, y) => { };
            OnBeforeExecute += (x, y) => { };
            OnAfterExecute += (x, y) => { };
        }
    }
}

using System;

namespace Z80.Core
{
    public class Processor
    {
        public IRegisters Registers { get; private set; }
        public IMemoryMap MemoryMap { get; private set; }
        public ushort AddressBus { get; private set; }
        public byte DataBus { get; private set; }
        public InterruptMode InterruptMode { get; private set; } = InterruptMode.Zero;

        public event EventHandler OnBeginInstructionCycle;
        public event EventHandler OnBeforeInstructionDataFetched;
        public event EventHandler<byte[]>  OnAfterInstructionDataFetched;
        public event EventHandler OnBeforeDecode;
        public event EventHandler<IInstruction> OnInstructionDecoded;
        public event EventHandler<IInstruction> OnBeforeInstructionExecuted;
        public event EventHandler<IInstruction> OnAfterInstructionExecuted;
        public event EventHandler OnEndInstructionCycle;

        public void Start()
        {

        }

        private void InstructionCycle()
        {
            // skeleton events for instruction cycle
            OnBeginInstructionCycle(this, EventArgs.Empty);
            OnBeforeInstructionDataFetched(this, EventArgs.Empty);
            OnAfterInstructionDataFetched(this, new byte[0]);
            OnBeforeDecode(this, EventArgs.Empty);
            OnInstructionDecoded(this, Instruction.NOP);
            OnBeforeInstructionExecuted(this, Instruction.NOP);
            OnAfterInstructionExecuted(this, Instruction.NOP);
            OnEndInstructionCycle(this, EventArgs.Empty);
        }

        public Processor()
        {
            Registers = new Registers();
            MemoryMap = new MemoryMap();
        }

        public Processor(IRegisters registers, IMemoryMap memoryMap)
        {
            Registers = registers;
            MemoryMap = memoryMap;
        }
    }
}

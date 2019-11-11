using System;

namespace Z80.Core
{
    public interface ITestProcessor
    {
        ushort AddressBus { get; }
        long ClockCycles { get; }
        byte DataBus { get; }
        long InstructionTicks { get; }
        InterruptMode InterruptMode { get; }
        bool InterruptsEnabled { get; }
        IMemory Memory { get; }
        IPorts Ports { get; }
        IRegisters Registers { get; }
        double SpeedInMhz { get; }
        IStack Stack { get; }
        ProcessorState State { get; }

        event EventHandler<ExecutionResult> AfterExecute;
        event EventHandler<InstructionPackage> BeforeExecute;

        void DisableInterrupts();
        void EnableInterrupts();
        void Halt();
        void RaiseInterrupt(Action callback);
        void RaiseNonMasktableInterrupt();
        void Reset(bool stopAfterReset = false);
        void Resume();
        void SetAddressBus(byte low, byte high);
        void SetAddressBus(ushort value);
        void SetDataBus(byte value);
        void SetInterruptMode(InterruptMode mode);
        void Start();
        void Stop();
        ExecutionResult ExecuteDirect(Instruction instruction, InstructionData data);
    }
}
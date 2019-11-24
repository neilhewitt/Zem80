using System;

namespace Z80.Core
{
    public interface IDebugProcessor
    {
        event EventHandler<ExecutionResult> AfterExecute;
        event EventHandler<InstructionPackage> BeforeExecute;
        event EventHandler BeforeStart;
        event EventHandler OnStop;
        event EventHandler OnHalt;

        ExecutionResult ExecuteDirect(Instruction instruction, InstructionData data);

        ushort AddressBus { get; }
        long ClockCycles { get; }
        byte DataBus { get; }
        long InstructionTicks { get; }
        InterruptMode InterruptMode { get; }
        bool InterruptsEnabled { get; }
        Memory Memory { get; }
        IPorts Ports { get; }
        IRegisters Registers { get; }
        double SpeedInMhz { get; }
        IStack Stack { get; }
        ProcessorState State { get; }

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
        void Start(bool synchronous = false);
        void Stop();

    }
}
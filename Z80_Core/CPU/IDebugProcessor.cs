using System;

namespace Z80.Core
{
    public interface IDebugProcessor
    {
        event EventHandler<ExecutionResult> AfterExecute;
        event EventHandler<ExecutionPackage> BeforeExecute;
        event EventHandler BeforeStart;
        event EventHandler OnStop;
        event EventHandler OnHalt;

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
        ProcessorState State { get; }

        void DisableInterrupts();
        void EnableInterrupts();
        void Halt();
        void RaiseInterrupt(Action callback);
        void RaiseNonMasktableInterrupt();
        void ResetAndClearMemory();
        void Resume();
        void Push(RegisterPairName register);
        void Pop(RegisterPairName register);
        ushort Peek();
        void SetAddressBus(byte low, byte high);
        void SetAddressBus(ushort value);
        void SetDataBus(byte value);
        void SetInterruptMode(InterruptMode mode);
        void Start(bool synchronous = false, ushort address = 0x0000);
        void Stop();
        ExecutionResult Execute(ExecutionPackage package);
    }
}
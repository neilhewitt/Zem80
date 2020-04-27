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
        event EventHandler<TickEvent> OnBeforeTick;
        event EventHandler<TickEvent> OnAfterTick;

        ExecutionResult Execute(ExecutionPackage package);
    }

    public class TickEvent
    {
        public Instruction Instruction { get; private set; }
        public MachineCycleType MachineCycleType { get; private set; }
        public int ClockCyclesAdded { get; private set; }

        public TickEvent(Instruction instruction, MachineCycleType machineCycleType, int clockCyclesAdded)
        {
            Instruction = instruction;
            MachineCycleType = machineCycleType;
            ClockCyclesAdded = clockCyclesAdded;
        }
    }
}